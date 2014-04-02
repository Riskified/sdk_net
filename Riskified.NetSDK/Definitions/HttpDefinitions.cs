using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Riskified.NetSDK.Definitions
{
    internal static class HttpDefinitions
    {
        public const string ShopDomainHeaderName = "X_RISKIFIED_SHOP_DOMAIN";
        public const string SubmitHeaderName = "X_RISKIFIED_SUBMIT_NOW";
        public const string HmacHeaderName = "X-RISKIFIED-HMAC-SHA256";

        public static string CalcHmac(string data, string authToken)
        {
            byte[] key = Encoding.ASCII.GetBytes(authToken);
            var myhmacsha256 = new HMACSHA256(key);
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            var stream = new MemoryStream(byteArray);
            string result = myhmacsha256.ComputeHash(stream).Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
            return result;
        }

    }
}
