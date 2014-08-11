using System;
using System.Text.RegularExpressions;
using System.Xml;
using Riskified.SDK.Exceptions;

namespace Riskified.SDK.Utils
{
    internal static class InputValidators
    {
        private static bool IsInputFullMatchingRegex(string value, string regex)
        {
            if (value == null)
                return false;
            var r = new Regex(regex);
            
            return r.IsMatch(value);
        }

        public static void ValidateEmail(string email)
        {
            if (!IsInputFullMatchingRegex(email, @"^([a-zA-Z0-9_\-\.\+\%]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})$"))
                throw new OrderFieldBadFormatException(string.Format("Email field invalid. Was \"{0}\"",email));
        }

        public static void ValidateIp(string ip)
        {
            if (!IsInputFullMatchingRegex(ip, @"^(?<First>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Second>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Third>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Fourth>2[0-4]\d|25[0-5]|[01]?\d\d?)$"))
                throw new OrderFieldBadFormatException(string.Format("IP field invalid. Was \"{0}\"", ip));
        }

        public static void ValidateCountryOrProvinceCode(string locationCode)
        {
            if (!IsInputFullMatchingRegex(locationCode, @"^[A-Za-z]{2}$"))
                throw new OrderFieldBadFormatException(string.Format("Location Code field invalid. Should be exactly 2 letters. Value was \"{0}\"", locationCode));
        }

        public static void ValidateAvsResultCode(string resultCode)
        {
            if (!IsInputFullMatchingRegex(resultCode, @"^[A-Z,a-z0-9]+$"))
                throw new OrderFieldBadFormatException(string.Format("Avs result Code field invalid. Should be exactly 1 letter. Value was \"{0}\"", resultCode));   
        }

        public static void ValidateCvvResultCode(string resultCode)
        {
            if (!IsInputFullMatchingRegex(resultCode, @"^[A-Za-z0-9]+$"))
                throw new OrderFieldBadFormatException(string.Format("Cvv result Code field invalid. Should be 1 letter or empty-string. Value was \"{0}\"", resultCode));
        }

        public static void ValidateValuedString(string stringToValidate, string fieldName)
        {
            if (string.IsNullOrEmpty(stringToValidate))
                throw new OrderFieldBadFormatException(fieldName + " can't be null or empty");
        }

        public static void ValidatePhoneNumber(string phoneNumber)
        {
            if (!IsInputFullMatchingRegex(phoneNumber, @"^\+?[0-9][0-9\-]+[0-9]$"))
                throw new OrderFieldBadFormatException(string.Format("Phone number format incorrect. Can only contain digits 0 to 9, the symbol '+' at the beginning and the symbol '-' between digits. Value was \"{0}\"", phoneNumber));
        }

        public static void ValidateZeroOrPositiveValue(double number, string fieldName)
        {
            if (number < 0)
                throw new OrderFieldBadFormatException(string.Format("{0} must be positive or zero. Value was \"{1}\"",fieldName, number));
        }

        public static void ValidatePositiveValue(int number, string fieldName)
        {
            if (number <= 0)
                throw new OrderFieldBadFormatException(string.Format("{0} must be positive (greater than zero). Value was \"{1}\"", fieldName, number));
        }

        public static void ValidateObjectNotNull(object obj, string fieldName)
        {
            if(obj == null)
                throw new OrderFieldBadFormatException(string.Format("{0} can't be null.", fieldName));
        }

        public static void ValidateDateNotDefault(DateTime date, string fieldName)
        {
            if(date.Equals(DateTime.MinValue) || date.Equals(DateTime.MaxValue))
                throw new OrderFieldBadFormatException(string.Format("{0} date value must have a valid logical date (not default value).", fieldName));
        }

        
        public static void ValidateCurrency(string currency)
        {
            if (string.IsNullOrEmpty(currency) || currency.Length != 3 || !IsInputFullMatchingRegex(currency,"^[A-Za-z]{3}$"))
                throw new OrderFieldBadFormatException("Currency must be a 3 letters code matching ISO 4217.");
        }

        public static void ValidateCreditCard(string creditCardNumber)
        {
            if(!IsInputFullMatchingRegex(creditCardNumber,@"^[Xx\-0-9]*[0-9]{4}$"))
                throw new OrderFieldBadFormatException("Credit card number should end with 4 digits, with preceeding sequence of digits and symbols of 'X','x',-'");

        }
    }
}
