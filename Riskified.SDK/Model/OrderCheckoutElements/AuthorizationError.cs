using Newtonsoft.Json;
using Riskified.SDK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model.OrderCheckoutElements
{
    [JsonObject("authorization_error")]
    public class AuthorizationError : IJsonSerializable
    {
        
        public AuthorizationError(DateTime? createdAt, string errorCode, string message = null)
        {
            this.CreatedAt = createdAt;
            this.ErrorCode = errorCode;

            // optional field
            this.Message = message;
        }

        public void Validate(bool isWeak = false)
        {
            InputValidators.ValidateObjectNotNull(CreatedAt, "Created At");
            InputValidators.ValidateDateNotDefault(CreatedAt.Value, "Created At");
            InputValidators.ValidateObjectNotNull(ErrorCode, "Error Code");

            if(!EnumUtil.GetDescriptions(typeof(AuthorizationErrorCode)).Contains(this.ErrorCode))
            {
                throw new Exceptions.OrderFieldBadFormatException("Authorization error code is not valid.");
            }
        }
        

        [JsonProperty(PropertyName = "created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty(PropertyName = "error_code")]
        public string ErrorCode { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
