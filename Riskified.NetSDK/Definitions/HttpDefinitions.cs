using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Riskified.NetSDK.Definitions
{
    internal class HttpDefinitions
    {
        public const string ShopDomainHeaderName = "X_RISKIFIED_SHOP_DOMAIN";
        public const string SubmitHeaderName = "X_RISKIFIED_SUBMIT_NOW";
        public const string HmacHeaderName = "X_RISKIFIED_HMAC_SHA256";

        public static string CalcHmac(string data, string authToken)
        {
            byte[] key = Encoding.ASCII.GetBytes(authToken);
            HMACSHA256 myhmacsha256 = new HMACSHA256(key);
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            MemoryStream stream = new MemoryStream(byteArray);
            string result = myhmacsha256.ComputeHash(stream).Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
            Console.WriteLine(result);
            return result;
        }
    }
}
