using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riskified.SDK.Model.OrderElements;

namespace Riskified.SDK.Utils
{
    /// <summary>
    /// Writes PaymentType like StringEnumConverter, except that the obsolete RewardPoints is sent as the
    /// documented rewards_points (the API doesn't expect reward_points). The compatibility switch keeps the
    /// 5.7.x reward_points.
    /// </summary>
    internal sealed class PaymentTypeConverter : StringEnumConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PaymentType) || objectType == typeof(PaymentType?);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
#pragma warning disable CS0618 // RewardPoints is obsolete; this is where it's translated
            var isRewardPoints = value is PaymentType paymentType && paymentType == PaymentType.RewardPoints;
#pragma warning restore CS0618

            if (isRewardPoints && !WireOptions.IsLegacy(serializer.Context))
            {
                base.WriteJson(writer, PaymentType.RewardsPoints, serializer);
                return;
            }

            base.WriteJson(writer, value, serializer);
        }
    }
}
