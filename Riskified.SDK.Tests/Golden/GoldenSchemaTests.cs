using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using NJsonSchema.Validation;
using Riskified.SDK.Utils;
using Xunit;

namespace Riskified.SDK.Tests.Golden
{
    /// <summary>
    /// Validates every golden sample against the request-body schema of each endpoint it's sent to,
    /// taken from the OpenAPI definitions in the API reference (Golden/Schema, refreshed by
    /// update-schemas.sh). Keys the docs don't list are allowed. Where the docs and the server
    /// disagree and the server accepts what the SDK sends, <see cref="LoadRequestSchema"/> adjusts
    /// the schema.
    /// </summary>
    public class GoldenSchemaTests
    {
        // Declared before SchemasBySample, which reads them: static fields initialize in order.
        private static readonly string[] OrderSchemas = ["pre-auth-decide", "post-auth-decide", "post-auth-submit"];
        private static readonly string[] CheckoutDeniedSchemas = ["pre-auth-checkout_denied", "post-auth-checkout_denied"];

        // Sample name -> schema files (Golden/Schema/<name>.json) for the endpoints it's sent to.
        // order_checkout goes to /checkout_create and /advise, which aren't part of the alignment.
        private static readonly Dictionary<string, string[]> SchemasBySample = new Dictionary<string, string[]>
        {
            ["order"] = OrderSchemas,
            ["order_checkout_denied"] = CheckoutDeniedSchemas,
            ["order_decision"] = ["pre-auth-decision"],
            ["order_5_7"] = OrderSchemas,
            ["order_checkout_denied_5_7"] = CheckoutDeniedSchemas,
            ["order_decision_5_7"] = ["pre-auth-decision"],
        };

        public static TheoryData<string, string> SampleSchemaPairs()
        {
            var data = new TheoryData<string, string>();
            foreach (var pair in SchemasBySample)
            {
                foreach (var schema in pair.Value)
                {
                    data.Add(pair.Key, schema);
                }
            }
            return data;
        }

        [Theory]
        [MemberData(nameof(SampleSchemaPairs))]
        public async Task Sample_MatchesApiSchema(string sampleName, string schemaName)
        {
            var schema = await LoadRequestSchema(schemaName);
            var json = HttpUtils.SerializeRequestBody(GoldenOrders.All[sampleName]);

            var errors = Flatten(schema.Validate(json)).Distinct().OrderBy(e => e).ToList();

            Assert.True(errors.Count == 0, "Schema errors:\n  " + string.Join("\n  ", errors));
        }

        [Fact]
        public void EverySampleWithAnEndpointInScopeIsValidated()
        {
            var unvalidated = GoldenOrders.All.Keys.Except(SchemasBySample.Keys).ToList();

            Assert.Equal(new[] { "order_checkout" }, unvalidated);
        }

        // OpenAPI 3.0 keeps schemas under components/schemas; NJsonSchema reads plain JSON Schema,
        // so the request-body schema becomes the root and the components become definitions.
        private static async Task<JsonSchema> LoadRequestSchema(string schemaName)
        {
            var definitionText = File.ReadAllText(Path.Combine(GoldenPaths.Schema, schemaName + ".json"))
                .Replace("#/components/schemas/", "#/definitions/");
            var definition = JObject.Parse(definitionText);

            // The docs type some dates as date-only (format: date), but the server accepts the full
            // date-time the SDK sends (question 9).
            foreach (var format in definition.Descendants().OfType<JProperty>().Where(p => p.Name == "format" && (string?)p.Value == "date").ToList())
            {
                format.Value = "date-time";
            }

            // recipient_ACH types created_at / updated_at as "number" with format date-time; the server
            // accepts the date-time string the SDK sends, like every other created_at (question 11).
            foreach (var property in definition.Descendants().OfType<JObject>()
                         .Where(p => p["type"] is JValue { Value: "number" } && p["format"] is JValue { Value: "date-time" }).ToList())
            {
                property["type"] = "string";
            }

            // Pre-auth /decide types available_balance as an integer, the post-auth pages as a number; the
            // server accepts the number the SDK sends (question 10).
            foreach (var balance in definition.Descendants().OfType<JProperty>()
                         .Where(p => p.Name == "available_balance" && p.Value is JObject schema && schema["type"] is JValue { Value: "integer" })
                         .Select(p => (JObject)p.Value).ToList())
            {
                balance["type"] = "number";
            }

            var requestSchema = (JObject)definition["paths"]!.Values().Single()
                .Values().Single()["requestBody"]!["content"]!["application/json"]!["schema"]!;

            var root = (JObject)requestSchema.DeepClone();
            root["definitions"] = definition["components"]!["schemas"]!.DeepClone();
            return await JsonSchema.FromJsonAsync(root.ToString());
        }

        // anyOf / oneOf failures carry one error list per branch (e.g. one per line item type).
        // Report only the branch the value matches best, otherwise every line item would also list
        // the required fields of every other line item type. A value outside a branch's enum
        // (product_type, payment_type) means it's the wrong branch, so those rank first and the
        // total error count breaks ties. allOf keeps every branch's errors.
        private static IEnumerable<string> Flatten(IEnumerable<ValidationError> errors)
        {
            foreach (var error in errors)
            {
                if (error is ChildSchemaValidationError childError && childError.Errors.Count > 0)
                {
                    var branches = childError.Errors.Values.Select(branch => Flatten(branch).ToList()).ToList();
                    var reported = error.Kind == ValidationErrorKind.NotAllOf
                        ? branches.SelectMany(branch => branch)
                        : branches
                            .OrderBy(branch => branch.Count(e => e.EndsWith(" " + ValidationErrorKind.NotInEnumeration)))
                            .ThenBy(branch => branch.Count)
                            .First();

                    foreach (var child in reported)
                    {
                        yield return child;
                    }
                }
                else
                {
                    yield return $"{error.Path} {error.Kind}";
                }
            }
        }
    }
}
