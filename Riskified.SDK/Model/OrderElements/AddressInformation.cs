using Newtonsoft.Json;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    
    public class AddressInformation : IJsonSerializable
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
            FirstName = firstName;
            LastName = lastName;
            Address1 = address1;
            City = city;
            Country = country;
            CountryCode = countryCode;
            Phone = phone;
            
            // optional fields:
            ProvinceCode = provinceCode;
            Address2 = address2;
            Province = province;
            ZipCode = zipCode;
            Company = company;
            FullName = fullName;
        }

        public void Validate(bool isWeak = false)
        {
            InputValidators.ValidateValuedString(FirstName, "First Name");
            if (!isWeak)
            {
                InputValidators.ValidateValuedString(LastName, "Last Name");
                InputValidators.ValidatePhoneNumber(Phone);
            }
            InputValidators.ValidateValuedString(Address1, "Address 1");
            InputValidators.ValidateValuedString(City, "City");
            InputValidators.ValidateValuedString(Country, "Country");
            InputValidators.ValidateCountryOrProvinceCode(CountryCode);

            // optional fields validations
            if (!string.IsNullOrEmpty(ProvinceCode))
            {
                InputValidators.ValidateCountryOrProvinceCode(ProvinceCode);
            }
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

        [JsonProperty(PropertyName = "last_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "name", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public string FullName { get; set; }

        [JsonProperty(PropertyName = "phone", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "province",Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public string Province { get; set; }

        [JsonProperty(PropertyName = "province_code", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public string ProvinceCode { get; set; }

        [JsonProperty(PropertyName = "zip", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public string ZipCode { get; set; }
    }
}
