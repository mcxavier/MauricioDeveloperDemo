using System;
using System.Collections.Generic;
using System.Linq;
using Infra.ExternalServices.Payments.Contracts;
using Infra.ExternalServices.Payments.Vendors;

namespace Infra.ExternalServices.Payments
{
    public class PaymentGatewayContext : IPaymentGatewayContext
    {
        private readonly IEnumerable<IPaymentGatewayServices> _providers;

        public PaymentGatewayContext(IEnumerable<IPaymentGatewayServices> providers)
        {
            this._providers = providers;
        }

        public IPaymentGatewayServices GetPaymentServices(GatewayProvider payment)
        {
            if (!Enum.IsDefined(typeof(GatewayProvider), payment))
            {
                return null;
            }

            return this._providers.SingleOrDefault(x => x.GetGatewayProvider() == payment);
        }
    }
}