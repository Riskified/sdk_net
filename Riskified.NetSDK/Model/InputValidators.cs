using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Riskified.NetSDK.Model
{
    public static class InputValidators
    {
        private static bool ValidateInputByRegex(string value, string regex)
        {
            var r = new Regex(regex);
            
            return r.IsMatch(value);
        }

        public static void ValidateEmail(string email)
        {
            if (!ValidateInputByRegex(email, @"^([a-zA-Z0-9_\-\.\+\%]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})$"))
                throw new ArgumentException(string.Format("Email field invalid. Was \"{0}\"",email));
        }

        public static void ValidateIp(string ip)
        {
            if (!ValidateInputByRegex(ip, @"^(?<First>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Second>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Third>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Fourth>2[0-4]\d|25[0-5]|[01]?\d\d?)$"))
                throw new ArgumentException(string.Format("IP field invalid. Was \"{0}\"", ip));
        }

        public static void ValidateCountryOrProvinceCode(string locationCode)
        {
            if (!ValidateInputByRegex(locationCode, @"^[A-Za-z]{2}$"))
                throw new ArgumentException(string.Format("Location Code field invalid. Should be exactly 2 letters. Value was \"{0}\"", locationCode));
        }

        public static void ValidateAvsResultCode(string resultCode)
        {
            if (!ValidateInputByRegex(resultCode, @"^[A-Za-z]$"))
                throw new ArgumentException(string.Format("Avs result Code field invalid. Should be exactly 1 letter. Value was \"{0}\"", resultCode));   
        }

        public static void ValidateCvvResultCode(string resultCode)
        {
            if (!ValidateInputByRegex(resultCode, @"^[A-Za-z]?$"))
                throw new ArgumentException(string.Format("Cvv result Code field invalid. Should be 1 letter or empty-string. Value was \"{0}\"", resultCode));
        }
    }
}
