using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    public class DigitalLineItem : LineItem 
    {
        public DigitalLineItem(
            // inherited
            string title,
            double price,
            int quantityPurchased,
            string productId = null,
            string sku = null,
            string condition = null,
            bool? requiresShipping = null,
            string category = null,
            string subCategory = null,
            string brand = null,
            Seller seller = null,
            DeliveredToType? deliveredTo = null,
            DateTime? deliveredAt = null,
            Policy policy = null,
            
            // giftcard specific
            string senderName = null,
            string displayName = null,
            bool? photoUploaded = null,
            string photoUrl = null,
            string greetingPhotoUrl = null,
            string message = null,
            string greetingMessage = null,
            string cardType = null,
            string cardSubType = null,
            string senderEmail = null,
            Recipient recipient = null
            ) : base(
                title: title, price: price, quantityPurchased: quantityPurchased, productId: productId, sku: sku,
                condition: condition,
                requiresShipping: requiresShipping, seller: seller, deliveredTo: deliveredTo, delivered_at: deliveredAt,
                category: category, subCategory: subCategory, brand: brand,
                productType: OrderElements.ProductType.Digital, policy: policy, message: message)
        {
            SenderName = senderName;
            DisplayName = displayName;
            PhotoUploaded = photoUploaded;
            PhotoUrl = photoUrl;
            GreetingPhotoUrl = greetingPhotoUrl;
            GreetingMessage = greetingMessage;
            CardType = cardType;
            CardSubtype = cardSubType;
            SenderEmail = senderEmail;
            Recipient = recipient;
        }

        public override void Validate(Validations validationType = Validations.Weak)
        {
            base.Validate(validationType);
        }

        /// <summary>
        /// The digital good's (giftcard) sender name.
        /// </summary>
        [JsonProperty(PropertyName = "sender_name")]
        public string SenderName { get; set; }

        /// <summary>
        /// The digital good's (giftcard) sender email.
        /// </summary>
        [JsonProperty(PropertyName = "sender_email")]
        public string SenderEmail { get; set; }

        /// <summary>
        /// The digital good's (giftcard) display name.
        /// </summary>
        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Is the gift card sender added a photo.
        /// </summary>
        [JsonProperty(PropertyName = "photo_uploaded")]
        public bool? PhotoUploaded { get; set; }

        /// <summary>
        /// The digital good's (giftcard) sender photo's url.
        /// </summary>
        [JsonProperty(PropertyName = "photo_url")]
        public string PhotoUrl { get; set; }

        /// <summary>
        /// The digital good's (giftcard) greeting's photo url.
        /// </summary>
        [JsonProperty(PropertyName = "greeting_photo_url")]
        public string GreetingPhotoUrl { get; set; }

        /// <summary>
        /// The digital good's (giftcard) message.
        /// </summary>
        [Obsolete]
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// The digital good's (giftcard) greeting message.
        /// </summary>
        [JsonProperty(PropertyName = "greeting_message")]
        public string GreetingMessage { get; set; }

        /// <summary>
        /// The digital good's (giftcard) type.
        /// </summary>
        [JsonProperty(PropertyName = "card_type")]
        public string CardType { get; set; }

        /// <summary>
        /// The digital good's (giftcard) sub type.
        /// </summary>
        [JsonProperty(PropertyName = "card_subtype")]
        public string CardSubtype { get; set; }

        [JsonProperty(PropertyName = "recipient")]
        public Recipient Recipient { get; set; }
    }
}
