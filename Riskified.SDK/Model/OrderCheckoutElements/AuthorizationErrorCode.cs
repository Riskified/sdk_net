using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model.OrderCheckoutElements
{
    public enum AuthorizationErrorCode
    {
        [Description("incorrect_number")] IncorrectNumber,
        [Description("invalid_number")] InvalidNumber,
        [Description("invalid_expiry_date")] InvalidExpiryDate,
        [Description("invalid_cvc")] InvalidCVC,
        [Description("expired_card")] ExpiredCard,
        [Description("incorrect_cvc")] IncorrectCVC,
        [Description("incorrect_zip")] IncorrectZip,
        [Description("incorrect_address")] IncorrectAddress,
        [Description("card_declined")] CardDeclined,
        [Description("processing_error")] ProcessingError,
        [Description("call_issuer")] CallIssuer,
        [Description("pick_up_card")] PickUpCard
    }
}
