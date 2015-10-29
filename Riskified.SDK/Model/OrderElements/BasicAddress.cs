using Newtonsoft.Json;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    public class BasicAddress : IJsonSerializable
    {
        /// <summary>
        /// Creates a BasicAddress instance
        /// </summary>
        /// <param name="address1">The address (street, house number)</param>
        /// <param name="city">The city part of the address</param>
        /// <param name="country">The full name of the country</param>
        /// <param name="countryCode">The 2 letter code of the country</param>
        /// <param name="phone">The phone number of the addressee</param>
        /// <param name="address2">Additional address information like entrance, apartment number, etc. (optional)</param>
        /// <param name="zipCode">The zipcode of the address (optional)</param>
        /// <param name="province">The full province name (optional)</param>
        /// <param name="provinceCode">The 2 letter code of the province (optional)</param>
        /// <param name="company">The company of the addressee (optional)</param>
        public BasicAddress(string address1 = null, string city = null, string country = null, string countryCode = null, string phone = null, string address2 = null, string zipCode = null, string province = null, string provinceCode = null, string company = null)
        {
            // optional fields:
            Address1 = address1;
            City = city;
            Country = country;
            CountryCode = countryCode;
            Phone = phone;
            ProvinceCode = provinceCode;
            Address2 = address2;
            Province = province;
            ZipCode = zipCode;
            Company = company;
        }

        /// <summary>
        /// Validates the objects fields content
        /// </summary>
        /// <param name="validationType">Validation level to use on this model</param>
        /// <exception cref="OrderFieldBadFormatException">throws an exception if one of the parameters doesn't match the expected format</exception>
        public virtual void Validate(Validations validationType = Validations.Weak)
        {
            // optional fields validations
            if(!string.IsNullOrEmpty(Phone))
            {
                InputValidators.ValidatePhoneNumber(Phone);
            }

            if (!string.IsNullOrEmpty(CountryCode))
            {
                InputValidators.ValidateCountryOrProvinceCode(CountryCode);
            }

            if (!string.IsNullOrEmpty(ProvinceCode))
            {
                InputValidators.ValidateCountryOrProvinceCode(ProvinceCode);
            }
        }

        [JsonProperty(PropertyName = "address1")]
        public string Address1 { get; set; }

        [JsonProperty(PropertyName = "address2")]
        public string Address2 { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "company")]
        public string Company { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "country_code")]
        public string CountryCode { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "province")]
        public string Province { get; set; }

        [JsonProperty(PropertyName = "province_code")]
        public string ProvinceCode { get; set; }

        [JsonProperty(PropertyName = "zip")]
        public string ZipCode { get; set; }
    }
}
