using System;
using System.Text.RegularExpressions;

namespace Infra.ExternalServices.Payments.Vendors.LinxPayhub.Util
{
    // TODO: aumentar a lista de cartoes válidos
    public static class UtilLinxPayHub
    {
        public static /*CardBrandType*/ string FindType(string cardNumber)
        {
            //if (Regex.Match(cardNumber, @"^4[0-9]{12}(?:[0-9]{3})?$").Success)
            //{
            //    return CardBrandType.Visa;
            //}

            //if (Regex.Match(cardNumber, @"^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$").Success)
            //{
            //    return CardBrandType.Mastercard;
            //}

            //if (Regex.Match(cardNumber, @"^3[47][0-9]{13}$").Success)
            //{
            //    return CardBrandType.AmericanExpress;
            //}

            ///*if (Regex.Match(cardNumber, @"^6(?:011|5[0-9]{2})[0-9]{12}$").Success) {
            //    return CardBrandType.Discover;
            //}*/

            //if (Regex.Match(cardNumber, @"^(?:2131|1800|35\d{3})\d{11}$").Success)
            //{
            //    return CardBrandType.Jcb;
            //}

            //throw new Exception("Unknown card.");
            return "";
        }
    }
}