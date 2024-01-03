using Api.App_Infra;
using Core.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using Core.QuerysCommands.Queries.Log;
using System.Linq;

namespace Api.Controllers
{
    [ServiceFilter(typeof(SmartSalesAuthorizeAttribute))]
    [Route("api/v1/[controller]"), ApiController]
    public class LogController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogList([FromQuery] GetLogByFilterQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(new Response { Payload = result.Logs }.SetPagerInfo(result.PagerInfo));
        }
    }
}