using Newtonsoft.Json;

namespace Riskified.NetSDK.Model
{
    // TODO add test classes for all model classes
    public class AddressInformation
    {
        public AddressInformation(string firstName, string lastName, string address1, string city, string country, string countryCode, string phone, string address2 = null, string zipCode = null, string province = null, string provinceCode = null, string company= null, string fullName = null)
        {
            InputValidators.ValidateCountryOrProvinceCode(countryCode);
            CountryCode = countryCode;
            Address1 = address1;
            Phone = phone;
            City = city;
            Country = country;
            FirstName = firstName;
            LastName = lastName;
            // optional fields:
            Address2 = address2;
            if (!string.IsNullOrEmpty(provinceCode))
            {
                InputValidators.ValidateCountryOrProvinceCode(provinceCode);
                ProvinceCode = provinceCode;
            }
            Province = province;
            ZipCode = zipCode;
            Company = company;
            FullName = fullName;
        }

        [JsonProperty(PropertyName = "address1", Required = Required.Always)]
        public string Address1 { get; set; }

        [JsonProperty(PropertyName = "address2",Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public string Address2 { get; set; }

        [JsonProperty(PropertyName = "city", Required=Required.Always)]
        public string City { get; set; }

        [JsonProperty(PropertyName = "company", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public string Company { get; set; }

        [JsonProperty(PropertyName = "country",Required = Required.Always)]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "country_code", Required = Required.Always)]
        public string CountryCode { get; set; }

        [JsonProperty(PropertyName = "first_name", Required = Required.Always)]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name", Required = Required.Always)]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "name", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public string FullName { get; set; }

        // TODO add validation for phone number
        [JsonProperty(PropertyName = "phone", Required = Required.Always)]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "province",Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public string Province { get; set; }

        [JsonProperty(PropertyName = "province_code", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public string ProvinceCode { get; set; }

        // TODO add validation for ZipCode
        [JsonProperty(PropertyName = "zip", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public string ZipCode { get; set; }
    }
}
