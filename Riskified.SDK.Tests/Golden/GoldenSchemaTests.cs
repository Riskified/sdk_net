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
    /// update-schemas.sh). Keys the docs don't list are allowed. Known gaps are listed in
    /// <see cref="KnownGaps"/>; remove an entry when the step that fixes it lands.
    /// </summary>
    public class GoldenSchemaTests
    {
        // Sample name -> schema files (Golden/Schema/<name>.json) for the endpoints it's sent to.
        // order_checkout goes to /checkout_create and /advise, which aren't part of the alignment.
        private static readonly Dictionary<string, string[]> SchemasBySample = new Dictionary<string, string[]>
        {
            ["order"] = ["pre-auth-decide", "post-auth-decide", "post-auth-submit"],
            ["order_checkout_denied"] = ["pre-auth-checkout_denied", "post-auth-checkout_denied"],
            ["order_decision"] = ["pre-auth-decision"],
        };

        private static readonly string[] OrderSchemas = ["pre-auth-decide", "post-auth-decide", "post-auth-submit"];
        private static readonly string[] CheckoutDeniedSchemas = ["pre-auth-checkout_denied", "post-auth-checkout_denied"
        ];

        // Each entry names the plan step (API_ALIGNMENT_PLAN.md) or open question that resolves it.
        private static readonly HashSet<string> KnownGaps =
        [
            .. new[]
            {
                Gap("order", OrderSchemas, "#/order.billing_address ArrayExpected", fixedBy: "step 3.1"),
                Gap("order", OrderSchemas, "#/order.shipping_address ArrayExpected", fixedBy: "step 3.1"),
                Gap("order", OrderSchemas, "#/order.charge_free_payment_details ArrayExpected", fixedBy: "step 3.2"),
                Gap("order", OrderSchemas, "#/order.line_items[6].pickup_address ArrayExpected", fixedBy: "step 3.3"),
                Gap("order", OrderSchemas, "#/order.line_items[6].dropoff_address ArrayExpected", fixedBy: "step 3.3"),
                Gap("order_checkout_denied", CheckoutDeniedSchemas,
                    "#/checkout.payment_details[1].authorization_error PropertyRequired", fixedBy: "steps 2.1 and 3.4"),
                Gap("order", OrderSchemas, "#/order.customer.kyc_details[0].update_at PropertyRequired",
                    fixedBy: "step 3.5"),
                Gap("order", OrderSchemas,
                    "#/order.line_items[0].seller.customer.kyc_details[0].update_at PropertyRequired",
                    fixedBy: "step 3.5"),
                Gap("order_checkout_denied", ["post-auth-checkout_denied"],
                    "#/checkout.customer.kyc_details[0].update_at PropertyRequired", fixedBy: "step 3.5"),
                Gap("order", OrderSchemas, "#/order.line_items[0].seller.starting_price StringExpected",
                    fixedBy: "step 3.8"),

                // The docs type these as date-only (format: date); the SDK sends a full date-time.
                Gap("order", OrderSchemas, "#/order.customer.date_of_birth DateExpected", fixedBy: "question 9"),
                Gap("order", OrderSchemas, "#/order.line_items[0].seller.customer.date_of_birth DateExpected",
                    fixedBy: "question 9"),
                Gap("order_checkout_denied", ["post-auth-checkout_denied"],
                    "#/checkout.customer.date_of_birth DateExpected", fixedBy: "question 9"),
                Gap("order", OrderSchemas, "#/order.passengers[0].date_of_birth DateExpected", fixedBy: "question 9"),
                Gap("order", OrderSchemas, "#/order.passengers[0].document_issue_date DateExpected",
                    fixedBy: "question 9"),
                Gap("order", OrderSchemas, "#/order.passengers[0].document_expiration_date DateExpected",
                    fixedBy: "question 9"),

                // Pre-auth /decide documents an integer, post-auth a number.
                Gap("order", ["pre-auth-decide"],
                    "#/order.payment_details[3].account_balance.available_balance IntegerExpected",
                    fixedBy: "question 10"),
            }.SelectMany(gap => gap)
        ];

        // fixedBy is documentation only; it isn't part of the matched string.
        private static IEnumerable<string> Gap(string sample, string[] schemas, string error, string fixedBy)
        {
            return schemas.Select(schema => $"{sample} {schema}: {error}");
        }

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
        public async Task Sample_MatchesApiSchema_ExceptKnownGaps(string sampleName, string schemaName)
        {
            var schema = await LoadRequestSchema(schemaName);
            var json = JsonConvert.SerializeObject(GoldenOrders.All[sampleName], HttpUtils.CreateSerializerSettings());

            var actual = Flatten(schema.Validate(json))
                .Select(error => $"{sampleName} {schemaName}: {error}")
                .ToHashSet();
            var expected = KnownGaps
                .Where(gap => gap.StartsWith($"{sampleName} {schemaName}: "))
                .ToHashSet();

            Assert.True(actual.SetEquals(expected),
                "New schema errors:\n  " + string.Join("\n  ", actual.Except(expected).OrderBy(e => e)) +
                "\nKnown gaps that no longer occur (remove them from KnownGaps):\n  " + string.Join("\n  ", expected.Except(actual).OrderBy(e => e)));
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
