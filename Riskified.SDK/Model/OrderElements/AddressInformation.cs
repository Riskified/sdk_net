using Newtonsoft.Json;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    
    public class AddressInformation : BasicAddress
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
        public AddressInformation(string firstName, string lastName, string address1, string city, string country, string countryCode, string phone, string address2 = null, string zipCode = null, string province = null, string provinceCode = null, string company= null, string fullName = null) :
            base(address1,city,country,countryCode,phone,address2,zipCode,province,provinceCode,company)
        {
            FirstName = firstName;
            LastName = lastName;
            
            // optional fields:
            FullName = fullName;
        }

        /// <summary>
        /// Validates the objects fields content
        /// </summary>
        /// <param name="validationType">Validation level to use on this model</param>
        /// <exception cref="OrderFieldBadFormatException">throws an exception if one of the parameters doesn't match the expected format</exception>
        public override void Validate(Validations validationType = Validations.Weak) 
        {
            base.Validate(validationType);

            if (validationType == Validations.All)
            {
                InputValidators.ValidateValuedString(FirstName, "First Name");
                InputValidators.ValidateValuedString(LastName, "Last Name");
                InputValidators.ValidatePhoneNumber(Phone); // make sure phone exists and has a value (addition to validation of BaseAddress)
            }

            InputValidators.ValidateValuedString(Address1, "Address 1");
            InputValidators.ValidateValuedString(City, "City");

            if(string.IsNullOrEmpty(Country) && string.IsNullOrEmpty(CountryCode))
            {
                throw new Exceptions.OrderFieldBadFormatException("Both Country or Country Code fields are missing - at least one should be speicified");
            }
        }

        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string FullName { get; set; }
    }
}
