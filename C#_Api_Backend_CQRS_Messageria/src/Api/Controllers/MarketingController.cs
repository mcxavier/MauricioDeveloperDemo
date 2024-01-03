using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Api.App_Infra;
using Core.Domains.Marketing.Commands.CreateNotificationCustomerWhenProductArrive;
using MediatR;
using Core.SharedKernel;

namespace Api.Controllers
{

    [Route("api/v1/[controller]"), ApiController, ServiceFilter(typeof(SmartSalesAuthorizeAttribute))]
    public class MarketingController : ControllerBase
    {

        private readonly IMediator _mediator;

        public MarketingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, Route("notify-when-product-arrive")]
        public async Task<IActionResult> NotifyWhenProductArrive([FromBody] CreateNotificationCustomerWhenProductArriveCommand content)
        {
            var commandResponse = await this._mediator.Send(content);

            var response = new Response
            {
                Message = commandResponse.Message,
                IsError = !(commandResponse.IsSuccess)
            };

            return Created("", response);
        }
    }
}