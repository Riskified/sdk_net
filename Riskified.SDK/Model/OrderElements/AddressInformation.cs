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
            if (isWeak)
            {
                if (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName))
                {
                    throw new Exceptions.OrderFieldBadFormatException("Both First name and last name are missing or empty - at least one should be specified");
                }
            }
            else
            {
                InputValidators.ValidateValuedString(FirstName, "First Name");
                InputValidators.ValidateValuedString(LastName, "Last Name");
                InputValidators.ValidatePhoneNumber(Phone);
            }

            InputValidators.ValidateValuedString(Address1, "Address 1");
            InputValidators.ValidateValuedString(City, "City");

            if(string.IsNullOrEmpty(Country) && string.IsNullOrEmpty(CountryCode))
            {
                throw new Exceptions.OrderFieldBadFormatException("Both Country or Country Code fields are missing - at least one should be speicified");
            }
            
            if (!string.IsNullOrEmpty(CountryCode))
            {
                InputValidators.ValidateCountryOrProvinceCode(CountryCode);
            }

            // optional fields validations
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

        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string FullName { get; set; }

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
