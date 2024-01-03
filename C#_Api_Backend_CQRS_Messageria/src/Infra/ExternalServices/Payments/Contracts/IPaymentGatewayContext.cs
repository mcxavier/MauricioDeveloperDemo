using Infra.ExternalServices.Payments.Vendors;

namespace Infra.ExternalServices.Payments.Contracts
{
    public interface IPaymentGatewayContext
    {
        IPaymentGatewayServices GetPaymentServices(GatewayProvider payment);
    }
}