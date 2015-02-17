using Newtonsoft.Json;
using Riskified.SDK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model.OrderElements
{
    public class Seller : IJsonSerializable
    {

        public Seller(Customer customer, int? correspondence = null, bool? priceNegotiated = null, float? startingPrice = null)
        {
            Customer = customer;

            // Optional fields
            Correspondence = correspondence;
            PriceNegotiated = priceNegotiated;
            StartingPrice = startingPrice;
        }


        public void Validate(Validations validationType = Validations.Weak)
        {
            InputValidators.ValidateObjectNotNull(Customer, "Customer");
            Customer.Validate(validationType);
            if(StartingPrice != null)
            {
                InputValidators.ValidateZeroOrPositiveValue((float)StartingPrice, "Starting price");
            }
        }

        /// <summary>
        /// A Customer object representing the seller.
        /// </summary>
        [JsonProperty(PropertyName = "customer")]
        public Customer Customer { get; set; }

        /// <summary>
        /// Number of messages sent between the customer and the seller.
        /// </summary>
        [JsonProperty(PropertyName = "correspondence")]
        public int? Correspondence { get; set; }

        /// <summary>
        /// True if the seller and customer negotiated the price between themselves.
        /// </summary>
        [JsonProperty(PropertyName = "price_negotiated")]
        public bool? PriceNegotiated { get; set; }

        /// <summary>
        /// The original price of the order, prior to any negotiation between seller and customer.
        /// </summary>
        [JsonProperty(PropertyName = "starting_price")]
        public float? StartingPrice { get; set; }


    }
}
