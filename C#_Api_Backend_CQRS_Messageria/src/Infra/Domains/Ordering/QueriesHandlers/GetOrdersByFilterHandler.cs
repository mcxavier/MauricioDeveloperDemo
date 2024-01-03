using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Core.Domains.Ordering.Dtos;
using Core.Models.Core.Ordering;
using Core.QuerysCommands.Queries.Orders.GetOrdersByFilter;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace Infra.QueryCommands.QueriesHandlers.Orders
{
    public class GetOrdersByFilterHandler : IRequestHandler<GetOrdersByFilterQuery, GetOrdersByFilterResponse>
    {
        private readonly SmartSalesIdentity _identity;
        private readonly CoreContext _context;

        public GetOrdersByFilterHandler(SmartSalesIdentity smartSalesIdentity, CoreContext context)
        {
            this._identity = smartSalesIdentity;
            this._context = context;
        }

        public async Task<GetOrdersByFilterResponse> Handle(GetOrdersByFilterQuery request, CancellationToken cancellationToken)
        { // TODO: passar a consulta para o dapper
            const int failedOrderStatus = (int) OrderStatusEnum.PaymentFailed;

            var orders = await Task.Run(() => this._context.Orders
                .Include(x => x.Customer)
                .Include(x => x.Shipping.Address)
                .Include(x => x.Status)
                .Include(x => x.Seller)
                .Include(x => x.Payment.PaymentType)
                .Where(order => (!request.Status.HasValue || order.StatusId == request.Status.Value) &&
                                (!request.ShippingType.HasValue || order.OrderTypeId == request.ShippingType.Value) &&
                                (!request.OrderDateFrom.HasValue || !order.CreatedAt.HasValue || order.CreatedAt >= request.OrderDateFrom) &&
                                (!request.OrderDateTo.HasValue || !order.CreatedAt.HasValue || order.CreatedAt <= request.OrderDateTo) &&
                                (order.Status.Id != failedOrderStatus) &&
                                (order.Payment.PaymentType.IsActive)
                )
                .Where(x => (string.IsNullOrEmpty(_identity.CurrentStoreCode) || x.StoreCode == _identity.CurrentStoreCode))
                .OrderBy(request.OrderBy + " " + request.OrderDirection)
                .Select(order => new OrderResumeDto
                {
                    Id = order.Id,
                    Reference = $"#{order.GetReference()}",
                    CustomerName = order.Customer.Name,
                    Address = order.Shipping.Address.StreetName,
                    CEP = order.Shipping.Address.ZipCode,
                    City = order.Shipping.Address.CityName,
                    State = order.Shipping.Address.StateName,
                    District = order.Shipping.Address.DistrictName,
                    Date = order.CreatedAt,
                    NetValue = order.Value,
                    Status = order.Status.Name,
                    StatusId = order.Status.Id,
                    Units = order.Items.Sum(x => x.Units),
                    Email = order.Customer.Email,
                    OrderType = order.OrderType.Id,
                    Phone = order.Customer.Phone,
                    CPF = order.Customer.Cpf,
                    RG = order.Customer.Rg,
                    BirthDay = order.Customer.BirthDay,
                    CardBrand = order.Payment.CardBrandTypeId,
                    SellerName = order.Seller == null ? (
                        order.SellerId == null ? null : 
                            this._context.Sellers.FirstOrDefault(x => x.Id == order.SellerId).Name)
                            : order.Seller.Name,
                    SellerId = order.SellerId
                }).ToPagedList(request.PageIndex, request.PageSize), cancellationToken);

            return new GetOrdersByFilterResponse(orders);
        }
    }
}