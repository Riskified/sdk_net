using Newtonsoft.Json;
using Riskified.SDK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model.OrderElements
{
    public class SocialDetails : IJsonSerializable
    {

        public SocialDetails(string network, string publicUsername, int? communityScore = null, string profilePictureUrl = null, string email = null, string bio = null,
                        string accountUrl = null, int? following = null, int? followed = null, int? posts = null)
        {
            Network = network;
            PublicUsername = publicUsername;
            
            // Optional fields
            CommunityScore = communityScore;
            ProfilePicture = profilePictureUrl;
            Email = email;
            Bio = bio;
            AccountUrl = accountUrl;
            Following = following;
            Followed = followed;
            Posts = posts;
        }


        public void Validate(bool isWeak = false)
        {
            InputValidators.ValidateValuedString(Network, "Network");
            InputValidators.ValidateValuedString(PublicUsername, "Network");

            if(!string.IsNullOrEmpty(Email))
            {
                InputValidators.ValidateEmail(Email);
            }
        }

        /// <summary>
        /// The name of the social network.
        /// </summary>
        [JsonProperty(PropertyName = "network")]
        public string Network { get; set; }

        /// <summary>
        /// The customer's public username.
        /// </summary>
        [JsonProperty(PropertyName = "public_username")]
        public string PublicUsername { get; set; }

        /// <summary>
        /// Aggregate community score for the customer.
        /// </summary>
        [JsonProperty(PropertyName = "community_score")]
        public int? CommunityScore { get; set; }

        /// <summary>
        /// URL of the customer's profile picture.
        /// </summary>
        [JsonProperty(PropertyName = "profile_picture")]
        public string ProfilePicture { get; set; }

        /// <summary>
        /// Customer's email address registered on the social network.
        /// </summary>
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Short biography of the customer on the social network.
        /// </summary>
        [JsonProperty(PropertyName = "bio")]
        public string Bio { get; set; }

        /// <summary>
        /// Direct URL to the customer's profile page on the social network.
        /// </summary>
        [JsonProperty(PropertyName = "account_url")]
        public string AccountUrl { get; set; }

        /// <summary>
        /// Number of users following the customer on the social network.
        /// </summary>
        [JsonProperty(PropertyName = "following")]
        public int? Following { get; set; }

        /// <summary>
        /// Number of users followed by the customer on the social network.
        /// </summary>
        [JsonProperty(PropertyName = "followed")]
        public int? Followed { get; set; }

        /// <summary>
        /// Number of posts published by the customer on the social network.
        /// </summary>
        [JsonProperty(PropertyName = "posts")]
        public int? Posts { get; set; }

    }
}
