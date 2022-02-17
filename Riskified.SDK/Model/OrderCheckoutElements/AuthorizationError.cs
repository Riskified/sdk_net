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

        public AuthorizationError(DateTimeOffset? createdAt, string errorCode, string message = null)
        {
            this.CreatedAt = createdAt;
            this.ErrorCode = errorCode;
            // optional field
            this.Message = message;
            
        }

        public void Validate(Validations validationType = Validations.Weak)
        {
            InputValidators.ValidateValuedString(ErrorCode, "Error Code");
            InputValidators.ValidateDateNotDefault(CreatedAt.Value, "Created At");

            // optional fields validations
            if (Message != null)
            {
                InputValidators.ValidateValuedString(Message, "Message");
            }
        }
        

        [JsonProperty(PropertyName = "created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        [JsonProperty(PropertyName = "error_code")]
        public string ErrorCode { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "drop_off")]
        public bool DropOff { get; set; }
    }
}
