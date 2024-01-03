using Core.Domains.Ordering.DomainServices;
using Core.Domains.Ordering.Models;
using Core.Models.Core.Payments;
using Core.Models.Core.Ordering;
using Core.SharedKernel;
using Core.SeedWork;
using CoreService.Infrastructure.Services;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using Infra.ExternalServices.Fiscal;
using Infra.ExternalServices.MailSender;
using Infra.ExternalServices.MailSender.Dtos;
using Infra.ExternalServices.Payments.Contracts;
using Infra.ExternalServices.Payments.Dtos;
using Infra.ExternalServices.Reshop.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utils.Extensions;
using Core.SharedKernel;
using Infra.ExternalServices.Payments.Vendors.PagarMe;

namespace Infra.QueryCommands.Commands.Orders
{
    public class GetOrderStatusCommandHandler : IRequestHandler<GetOrderStatusCommand, Response>
    {
        private readonly CoreContext _context;
        private readonly ILogger<GetOrderStatusCommandHandler> _logger;
        private readonly SmartSalesIdentity _identity;
        private readonly IPaymentGatewayServices _gateway;
        //private readonly PagarMeGatewayServices _gateway;
        private readonly IPagarMeOrderIntegrationService _pagarMeIntegration;

        //PagarMeGatewayServices gateway,
        public GetOrderStatusCommandHandler(CoreContext context,
                                        ILogger<GetOrderStatusCommandHandler> logger,
                                        SmartSalesIdentity identity,
                                        IPaymentGatewayServices gateway,
                                        IPagarMeOrderIntegrationService pagarMeIntegration)
        {
            _context = context;
            _logger = logger;
            _identity = identity;
            _gateway = gateway;
            _pagarMeIntegration = pagarMeIntegration;
        }


        public async Task<Payment> GetNewPayment(string orderCode, Payment oldPayment)
        {

            ServiceResponse<PaymentDto> response = await _gateway.GetOrderStatus(orderCode);
            PaymentDto paymentResponse = response.ResponseData;

            oldPayment.StatusId = Enumeration.Cast<PaymentStatus>(paymentResponse.Status).Id;

            return oldPayment;
        }
        public async Task<Order> GetNewOrder(Order oldOrder)
        {
            Order newOrder = oldOrder; // Copying reference

            Payment oldPayment = await _context.Payments.FirstOrDefaultAsync(x => x.Id == oldOrder.PaymentId);

            if (oldPayment == null)
            {
                throw new Exception("Database is inconsistent with order and payment mismatch.");
            }

            Payment newPayment = await GetNewPayment(oldOrder.OrderCode, oldPayment);

            // Updating order status by payment status
            if (newPayment.StatusId == PaymentStatus.Failed.Id) newOrder.StatusId = OrderStatus.Cancelled.Id;
            if (newPayment.StatusId == PaymentStatus.Processed.Id) newOrder.StatusId = OrderStatus.Paid.Id;
            if (newPayment.StatusId == PaymentStatus.Processing.Id) newOrder.StatusId = OrderStatus.AwaitingValidation.Id;

            await _context.SaveChangesAsync();

            return newOrder;
        }

        public async Task<Response> Handle(GetOrderStatusCommand request, CancellationToken cancellationToken)
        {

            this._logger.LogInformation("Mapping order from {request} ({@request})", nameof(GetOrderStatusCommand), request);

            Order newOrder = await GetNewOrder(request.Order);

            this._context.Orders.Update(newOrder);

            this._logger.LogInformation("Saving order in database ...");

            await _context.SaveChangesAsync();
            newOrder.Payment.Order = null;

            return new Response(newOrder);
        }
    }
}
