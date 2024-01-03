using System.Collections.Generic;
using System.Text.RegularExpressions;
using Core.Domains.Ordering.Models;
using Core.SeedWork;
using Core.SharedKernel;

namespace Core.Models.Core.Payments
{
    public class Payment : Entity<int>, IAggregateRoot, IStoreReferenced
    {
        public string StoreCode { get; set; }
        public string Pan { get; set; }
        public int? CardBrandTypeId { get; set; }
        public PaymentCardBrandType CardBrandType { get; set; }
        public long Amount { get; set; }
        public long Installments { get; set; }
        public string Currency { get; set; }
        public string GatewayProvider { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int StatusId { get; set; }
        public PaymentStatus Status { get; set; }
        public int PaymentTypeId { get; set; }
        public PaymentType PaymentType { get; set; }
        public IList<PaymentTransaction> Transactions { get; set; }

        public CardBrandEnum GetCardBrand()
        {
            var panTrated = this.Pan.Replace("*", "");

            if (Regex.Match(panTrated, @"^4[0-9]{12}(?:[0-9]{3})?$").Success)
            {
                return CardBrandEnum.Visa;
            }

            if (Regex.Match(panTrated, @"^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$").Success)
            {
                return CardBrandEnum.Mastercard;
            }

            if (Regex.Match(panTrated, @"^3[47][0-9]{13}$").Success)
            {
                return CardBrandEnum.AmericanExpress;
            }

            if (Regex.Match(panTrated, @"^6(?:011|5[0-9]{2})[0-9]{12}$").Success)
            {
                return CardBrandEnum.Discover;
            }

            if (Regex.Match(panTrated, @"^(?:2131|1800|35\d{3})\d{11}$").Success)
            {
                return CardBrandEnum.Jcb;
            }

            return CardBrandEnum.Uknown;
        }

        public string GetSecurityPan()
        {
            return this.Pan.Substring(12);
        }
    }
}