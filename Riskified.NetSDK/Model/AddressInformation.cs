using Newtonsoft.Json;

namespace Riskified.NetSDK.Model
{
    // TODO add test classes for all model classes
    public class AddressInformation
    {
        private string _countryCode;
        private string _provinceCode;

        [JsonProperty(PropertyName = "address1", Required = Required.Always)]
        public string Address1 { get; set; }

        [JsonProperty(PropertyName = "address2", Required = Required.Default)]
        public string Address2 { get; set; }

        [JsonProperty(PropertyName = "city", Required=Required.Always)]
        public string City { get; set; }

        [JsonProperty(PropertyName = "company", Required = Required.Default)]
        public string Company { get; set; }

        [JsonProperty(PropertyName = "country",Required = Required.Always)]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "country_code", Required = Required.Always)]
        public string CountryCode
        {
            get { return _countryCode; }
            set
            {
                InputValidators.ValidateCountryOrProvinceCode(value);
                _countryCode = value;
            }
        }
        //TODO generaling for all model - make sure no empty strings are inserted
        [JsonProperty(PropertyName = "first_name", Required = Required.Always)]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name", Required = Required.Always)]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "name", Required = Required.Default)]
        public string FullName { get; set; }

        // TODO add validation for phone number
        [JsonProperty(PropertyName = "phone", Required = Required.Always)]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "province",Required = Required.Default)]
        public string Province { get; set; }

        [JsonProperty(PropertyName = "province_code", Required = Required.Default)]
        public string ProvinceCode
        {
            get { return _provinceCode; }
            set
            {
                InputValidators.ValidateCountryOrProvinceCode(value);
                _provinceCode = value;
            }
        }

        // TODO add validation for ZipCode
        [JsonProperty(PropertyName = "zip", Required = Required.Default)]
        public string ZipCode { get; set; }
    }
}
