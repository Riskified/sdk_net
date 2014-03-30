using Newtonsoft.Json;

namespace Riskified.NetSDK.Model
{
    public class PaymentDetails
    {

        [JsonProperty(PropertyName = "avs_result_code", Required = Required.Always)]
        public string AvsResultCode { get; set; }

        [JsonProperty(PropertyName = "credit_card_bin", Required = Required.Always)]
        public string CreditCardBin { get; set; }

        [JsonProperty(PropertyName = "credit_card_company", Required = Required.Always)]
        public string CreditCardCompany { get; set; }

        [JsonProperty(PropertyName = "credit_card_number", Required = Required.Always)]
        public string CreditCardNumber { get; set; }

        [JsonProperty(PropertyName = "cvv_result_code", Required = Required.Always)]
        public string CvvResultCode { get; set; }
    }

}
