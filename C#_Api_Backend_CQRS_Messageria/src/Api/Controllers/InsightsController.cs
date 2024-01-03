using Api.App_Infra;
using Core.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using Core.QuerysCommands.Queries.Insights;

namespace Api.Controllers
{
    [ServiceFilter(typeof(SmartSalesAuthorizeAttribute))]
    [Route("api/v1/[controller]"), ApiController]
    public class InsightsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InsightsController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetInsightsList([FromQuery] GetInsightsByFilterQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(new Response { Payload = result.Insights });
        }
    }
}