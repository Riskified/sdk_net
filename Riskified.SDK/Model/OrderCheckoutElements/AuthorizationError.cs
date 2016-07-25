using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riskified.SDK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model.OrderCheckoutElements
{
    public class AuthorizationError : IJsonSerializable
    {

        public AuthorizationError(DateTime createdAt, AuthorizationErrorCode errorCode, string message = null)
        {
            this.CreatedAt = createdAt;
            this.ErrorCode = errorCode;

            // optional field
            this.Message = message;
        }

        public void Validate(Validations validationType = Validations.Weak)
        {
            InputValidators.ValidateObjectNotNull(ErrorCode, "Error Code");

        }
        

        [JsonProperty(PropertyName = "created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty(PropertyName = "error_code")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AuthorizationErrorCode ErrorCode { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
