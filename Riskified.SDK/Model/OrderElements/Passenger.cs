using Newtonsoft.Json;
using Riskified.SDK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model.OrderElements
{
    [JsonObject("passenger")]
    public class Passenger : IJsonSerializable
    {
        /// <summary>
        /// The shipping line (shiiping method)
        /// </summary>
        /// <param name="price">The price of this shipping method</param>
        /// <param name="title">A human readable name for the shipping method</param>
        /// <param name="code">A code to the shipping method</param>
        public Passenger(string firstname, string lastname, DateTime dateOfBirth, string nationalityCode,
                         float? insurancePrice, string documentNumber, string documentType, DateTime? documentIssueDate,
                         DateTime? documentExpirationDate, string passengerType = null, string insuranceType = null)
        {
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.DateOfBirth = dateOfBirth;
            this.nationalityCode = nationalityCode;
            this.insuranceType = insuranceType;
            this.insurancePrice = insurancePrice;
            this.DocumentNumber = documentNumber;
            this.DocumetType = documentType;
            this.DocumentIssueDate = documentIssueDate;
            this.DocumentExpirationDate = documentExpirationDate;
            this.PassengerType = passengerType;
        }

        /// <summary>
        /// Validates the objects fields content
        /// </summary>
        /// <param name="validationType">Should use weak validations or strong</param>
        /// <exception cref="OrderFieldBadFormatException">throws an exception if one of the parameters doesn't match the expected format</exception>
        public void Validate(Validations validationType = Validations.Weak)
        {
            //TODO: add validations
        }

        [JsonProperty(PropertyName = "first_name")]
        public string Firstname { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string Lastname { get; set; }

        [JsonProperty(PropertyName = "date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        [JsonProperty(PropertyName = "nationality_code")]
        public string nationalityCode { get; set; }

        [JsonProperty(PropertyName = "insurance_type")]
        public string insuranceType { get; set; }

        [JsonProperty(PropertyName = "insurance_price")]
        public float? insurancePrice { get; set; }

        [JsonProperty(PropertyName = "document_number")]
        public string DocumentNumber { get; set; }

        [JsonProperty(PropertyName = "document_type")]
        public string DocumetType { get; set; }

        [JsonProperty(PropertyName = "document_issue_date")]
        public DateTime? DocumentIssueDate { get; set; }

        [JsonProperty(PropertyName = "document_expiration_date")]
        public DateTime? DocumentExpirationDate { get; set; }

        [JsonProperty(PropertyName = "passenger_type")]
        public string PassengerType { get; set; }


    }

}

