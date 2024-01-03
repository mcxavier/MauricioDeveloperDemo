using Api.App_Infra;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Core.QuerysCommands.Queries.Customers.GetCustomersByFilter;
using MediatR;
using Core.SharedKernel;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Core.Domains.Customers.Dtos;

namespace Api.Controllers
{

    [ServiceFilter(typeof(SmartSalesAuthorizeAttribute))]
    [Route("api/v1/[controller]"), ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomersController> _logger;
        private readonly IConfiguration _configuration;

        public CustomersController(ILogger<CustomersController> logger, IMediator mediator)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomersList([FromQuery] GetCustomersByFilterQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(new Response { Payload = result.Customers.ToList(), }.SetPagerInfo(result.Customers));
        }

        [HttpGet, Route("cep/{pCep}")]
        public async Task<IActionResult> GetCepAsync(string pCep)
        {
            var result = await _mediator.Send(new CepDto() { numCep = pCep });
            var response = new Response { Payload = result.Payload };

            return Ok(response);
        }
    }
}