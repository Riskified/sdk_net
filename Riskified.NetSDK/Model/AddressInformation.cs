using Newtonsoft.Json;
using Riskified.NetSDK.Utils;

namespace Riskified.NetSDK.Model
{
    // TODO add test classes for all model classes
    public class AddressInformation
    {
        /// <summary>
        /// Creates an AddressInformation instance
        /// </summary>
        /// <param name="firstName">The first name of the addressee</param>
        /// <param name="lastName">The last name of the addressee</param>
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
        /// <param name="fullName">The full name of the addressee (optional)</param>
        public AddressInformation(string firstName, string lastName, string address1, string city, string country, string countryCode, string phone, string address2 = null, string zipCode = null, string province = null, string provinceCode = null, string company= null, string fullName = null)
        {
            InputValidators.ValidateValuedString(firstName, "First Name");
            FirstName = firstName;
            InputValidators.ValidateValuedString(lastName, "Last Name");
            LastName = lastName;
            InputValidators.ValidateValuedString(address1, "Address 1");
            Address1 = address1;
            InputValidators.ValidateValuedString(city, "City");
            City = city;
            InputValidators.ValidateValuedString(country, "Country");
            Country = country;
            InputValidators.ValidateCountryOrProvinceCode(countryCode);
            CountryCode = countryCode;
            InputValidators.ValidatePhoneNumber(phone);
            Phone = phone;
            
            // optional fields:
            
            if (!string.IsNullOrEmpty(provinceCode))
            {
                InputValidators.ValidateCountryOrProvinceCode(provinceCode);
                ProvinceCode = provinceCode;
            }
            Address2 = address2;
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
