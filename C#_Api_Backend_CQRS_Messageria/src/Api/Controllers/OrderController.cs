using Api.App_Infra;
using Api.ViewModels;
using Core.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models.Core.Payments;
using Core.QuerysCommands.Commands.Orders.ChangeOrderStatus;
using Core.QuerysCommands.Commands.Orders.ChangeSeller;
using Core.QuerysCommands.Queries.Orders.GetOrdersByFilter;
using CoreService.Infrastructure.Services;
using Infra.EntitityConfigurations.Contexts;
using MediatR;
using Infra.QueryCommands.Commands.Orders;
using Infra.ExternalServices.Payments.Vendors.PagarMe.Util;

namespace Api.Controllers
{
    [ApiController, Route("api/v1/order"), ServiceFilter(typeof(SmartSalesAuthorizeAttribute))]
    public class OrderController : ControllerBase
    {
        private readonly ICampaignIntegrationService _campaignService;
        private readonly ILogger<OrderController> _logger;
        private readonly CoreContext _context;
        private readonly IMediator _mediator;

        public OrderController(ICampaignIntegrationService campaignService, ILogger<OrderController> logger, CoreContext context, IMediator mediator)
        {
            this._campaignService = campaignService;
            this._mediator = mediator;
            this._context = context;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetByFilter([FromQuery] GetOrdersByFilterQuery query)
        {

            var ordersQuery = await this._mediator.Send(query);

            return Ok(new Response(ordersQuery.Orders));
        }

        [HttpGet, Route("card-brand/{binId}")]
        public async Task<IActionResult> GetCardBrand([FromRoute] string binId)
        {
            var ret = UtilPagarMe.FindType((binId + "0000000000000000").Substring(0, 16));

            ObjectResult route_response = BadRequest(new Response());
            route_response = Ok(ret);
            return route_response;
        }

        [HttpPost, Route("change-orders-status")]
        public async Task<IActionResult> ChangeOrdersStatus([FromBody] ChangeOrderStatusCommand command)
        {
            var response = await _mediator.Send(command);

            if (response.IsSuccess)
            {
                return Ok(new Response(response));
            }

            return BadRequest(new Response(response));
        }

        [HttpPost, Route("change-seller")]
        public async Task<IActionResult> ChangeSeller([FromBody] ChangeSellerCommand command)
        {
            var response = await _mediator.Send(command);

            if (response.IsSuccess)
            {
                return Ok(new Response(response));
            }

            return BadRequest(new Response(response));
        }

        private string PaymentTypeFromInt(int type)
        {
            PaymentTypeEnum e = (PaymentTypeEnum)type;
            switch (e)
            {
                case PaymentTypeEnum.CreditCard: return PaymentType.CreditCard.ToString();
                case PaymentTypeEnum.DebitCard: return PaymentType.DebitCard.ToString();
                case PaymentTypeEnum.PaymentSlip: return PaymentType.PaymentSlip.ToString();
                case PaymentTypeEnum.Pix: return PaymentType.Pix.ToString();
                default: return "Desconhecido";
            }
        }

        [HttpGet, Route("{orderId}/products")]
        public IActionResult GetOrderProducts(int orderId)
        {
            var order = this._context.Orders
              .Include("Items.ProductVariation.Specifications")
              .Include("Items.ProductVariation.Images")
              .Include("Seller")
              .Include(x => x.Payment)
              .FirstOrDefault(order => order.Id == orderId);

            var items = order?.Items?.Select(item =>
            {
                var size = item.ProductVariation.Specifications.Any(specification => specification.TypeId == 1) ?
                  item.ProductVariation.Specifications.FirstOrDefault(specification => specification.TypeId == 1).Description : "-";

                var color = item.ProductVariation.Specifications.Any(specification => specification.TypeId == 2) ?
                  item.ProductVariation.Specifications.FirstOrDefault(specification => specification.TypeId == 2).Value : "-";

                var images = item.ProductVariation.Images.FirstOrDefault()?.UrlImage;

                var sku = item.ProductVariation.StockKeepingUnit;

                return new OrderProductDetailViewModel
                {
                    Id = item.Id,
                    Name = item.ProductName,
                    Size = size,
                    Color = color,
                    NetValue = item.GetNetValue(),
                    Quantity = item.Units,
                    Image = images,
                    Sku = sku
                };
            })?.ToList();

            if (items == null)
            {
                return NotFound(new Response
                {
                    Message = "Nenhum Pedido encontrado",
                    Errors = new List<Error>(){
                        new Error(404, "Nenhum Pedido encontrado")
                    }
                });
            }

            if (!items.Any())
            {
                return NotFound(new Response
                {
                    Message = "Nenhum Produto encontrado no pedido",
                    Errors = new List<Error>(){
                        new Error(404, "Nenhum Produto encontrado no pedido")
                    },
                    Payload = null
                });
            }

            var viewModel = new OrderDetailViewModel();
            viewModel.OrderItens = items;
            viewModel.PaymentType = PaymentTypeFromInt(order?.Payment?.PaymentTypeId ?? 0);
            viewModel.OrderGrossValue = order.GetItemsGrossValues();
            viewModel.OrderNetValue = order.GetNetTotal();
            viewModel.Discount = order.GetDiscounts();
            viewModel.CardNumber = order?.Payment?.Pan?.Substring(12);
            viewModel.ShippingAmmount = order.ShippingAmmount;
            viewModel.Installments = (int)(order?.Payment?.Installments ?? 1);
            viewModel.SellerId = (int)(order?.SellerId ?? 0);
            viewModel.SellerName = (string)(order?.Seller?.Name ?? "Nenhum");

            var response = new Response
            {
                Message = "Produtos encontrados",
                Payload = viewModel,
            };

            return Ok(response);
        }

        [HttpGet, Route("{orderId}/payments")]
        public IActionResult GetOrderPayments(int orderId)
        {
            var payments = this._context.Payments.FirstOrDefault(payment => payment.OrderId == orderId);

            var response = new Response
            {
                Payload = payments
            };

            return Ok(response);
        }

        [HttpGet, Route("status-types")]
        public async Task<IActionResult> GetOrderStatusTypes()
        {
            var result = await _context.OrderStatus.Where(x => x.Id != 7).ToListAsync();
            var response = new Response
            {
                Payload = result
            };

            return Ok(response);
        }

        [HttpGet, Route("shipping-types")]
        public async Task<IActionResult> GetOrderShippingTypes()
        {
            var result = await _context.OrderType.ToListAsync();

            var types = new Dictionary<int, string> {
                { 1, "Entrega" },
                { 2, "Retirada na loja" }
            };

            result = result.Select(shippingType =>
            {
                if (types.TryGetValue(shippingType.Id, out var value))
                {
                    shippingType.Name = value;
                }
                return shippingType;
            }).ToList();

            var response = new Response
            {
                Payload = result
            };

            return Ok(response);
        }

        [HttpGet, Route("installments")]
        public async Task<IActionResult> GetOrderInstallments()
        {
            var types = new List<dynamic> {
                new { Id = 1, Value = 1, Description = "1x sem juros" },
                new { Id = 2, Value = 2, Description = "2x sem juros" },
                new { Id = 3, Value = 3, Description = "3x sem juros" },
                new { Id = 4, Value = 4, Description = "4x sem juros" },
                new { Id = 5, Value = 5, Description = "5x sem juros" },
                new { Id = 6, Value = 6, Description = "6x sem juros" },
            };           

            var response = new Response
            {
                Payload = types
            };

            return Ok(response);
        }

        [HttpGet, Route("payment-type")]
        public async Task<IActionResult> GetPaymentTypes()
        {
            var result = await _context.PaymentTypes.Where(x => x.IsActive).ToListAsync();
            var response = new Response
            {
                Payload = result
            };

            return Ok(response);
        }

        [HttpGet, Route("card-brand-type")]
        public async Task<IActionResult> GetCardBrandTypes()
        {
            var result = await _context.CardBrandTypes.ToListAsync();
            var response = new Response
            {
                Payload = result
            };

            return Ok(response);
        }

        [HttpGet, Route("{orderId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int orderId)
        {
            var order = await this._context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (order == null)
            {
                return NotFound(new Response
                {
                    Message = "Pedido não encontrado",
                    Errors = new List<Error>(){
                        new Error(404, "Pedido não encontrado")
                    }
                });
            }

            GetOrderStatusCommand request = new()
            {
                Order = order
            };

            var response = await _mediator.Send(request);

            if (response.IsError)
            {
                return NotFound(new Response
                {
                    Message = "Pedido não encontrado no gateway",
                    Errors = new List<Error>(){
                        new Error(404, "Pedido não encontrado no gateway")
                    }
                });
            }

            return Ok(new Response
            {
                Message = "Pedido Encontrado",
                Payload = response.Payload
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand request)
        {
            try
            {
                var response = await _mediator.Send(request);
                OrderResponse payload = (OrderResponse)response.Payload;

                return Ok(new Response
                {
                    Message = "Pedido cadastrado",
                    Payload = new
                    {
                        GeneratedId = payload.OrderId,
                        Reference = payload.Reference,
                        AuthenticationUrl = payload.AuthenticationUrl,
                        QRCode = payload.QRCode,
                        QRCodeUrl = payload.QRCodeUrl
                    }
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                int errorCode = 500;

                if (e.Message.Contains("Authorization has been denied for this request"))
                {
                    errorCode = 401;
                }
                else if (e.Message.Contains("An exception was thrown while attempting to evaluate a LINQ query parameter expression") ||
                         e.Message.Contains("Object reference not set to an instance of an object.") || e.Message.Contains("Value cannot be null."))
                {
                    errorCode = 400;
                }

                return StatusCode(errorCode, new Response
                {
                    Message = e.Message,
                    Errors = new List<Error> { new Error(errorCode, e.Message) },
                    IsError = true
                });
            }
        }
    }
}