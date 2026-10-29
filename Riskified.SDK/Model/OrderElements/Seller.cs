using Newtonsoft.Json;
using Riskified.SDK.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
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
        /// Sent as a string (e.g. "120.00"), as the API documents it.
        /// </summary>
        [JsonIgnore]
        public float? StartingPrice { get; set; }

        private bool _legacyWireFormat;

        // starting_price is documented as a float-formatted string. Two decimals at least, like the docs'
        // examples, plus any further digits; invariant culture so it's never "120,00". The compatibility
        // switch sends the 5.7.x number.
        [JsonProperty(PropertyName = "starting_price")]
        private object StartingPriceWire
        {
            get
            {
                if (StartingPrice == null)
                {
                    return null;
                }

                return _legacyWireFormat
                    ? (object)StartingPrice
                    : ((decimal)StartingPrice.Value).ToString("0.00#####", CultureInfo.InvariantCulture);
            }
            set
            {
                // Deserializing an object-typed member yields a string or a boxed number; accept either.
                if (value is string text)
                {
                    StartingPrice = float.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture);
                }
                else if (value != null)
                {
                    StartingPrice = Convert.ToSingle(value, CultureInfo.InvariantCulture);
                }
            }
        }

        [OnSerializing]
        private void OnSerializingSeller(StreamingContext context)
        {
            _legacyWireFormat = WireOptions.IsLegacy(context);
        }


    }
}
