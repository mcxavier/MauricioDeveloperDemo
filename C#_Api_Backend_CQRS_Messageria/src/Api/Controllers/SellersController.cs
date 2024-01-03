using Api.App_Infra;
using Core.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Infra.EntitityConfigurations.Contexts;
using Core.Models.Core.Customers;
using System;
using System.Linq;

namespace Api.Controllers
{
    [ServiceFilter(typeof(SmartSalesAuthorizeAttribute))]
    [Route("api/v1/[controller]"), ApiController]
    public class SellersController : ControllerBase
    {
        public readonly CoreContext _context;
        public SellersController(CoreContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _context.Sellers.Where(x => x.IsActive).ToListAsync();
            var response = new Response { Payload = result };

            return Ok(response);
        }


        [HttpPost, Route("salesAgent")]
        public async Task<IActionResult> PostSalesAgent([FromBody] Seller SalesAgent)
        {
            try
            {
                var result = await _context.Sellers.AddAsync(SalesAgent);
                var response = new Response { Payload = result };
                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, new Response
                {
                    IsError = true,
                    Message = "Ocorreram problemas ao atualizar o Vendedor [" + e.Message + "].",
                    Payload = new object[] { },
                });
            }
        }
    }
}