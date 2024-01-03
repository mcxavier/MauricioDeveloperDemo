using Api.App_Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoreLinq.Extensions;
using System.Linq;
using System.Threading.Tasks;
using Core.Domains.Catalogs.Queries;
using Core.Domains.Catalogs.Requests;
using Infra.EntitityConfigurations.Contexts;
using Infra.Domains.Catalogs;
using Core.SharedKernel;
using System;
using Infra.Responses;

namespace Api.Controllers
{

    [ServiceFilter(typeof(SmartSalesAuthorizeAttribute))]
    [ApiController, Route("api/v1/catalog")]
    public class CatalogController : ControllerBase
    {
        private readonly CoreContext _context;
        private readonly IdentityContext _identityContext;
        private readonly ICatalogService _catalogService;

        public CatalogController(CoreContext context, IdentityContext identityContext, ICatalogService catalogService)
        {
            _context = context;
            _identityContext = identityContext;
            _catalogService = catalogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCatalogs([FromQuery] FilterCatalogQuery catalogQuery, [FromHeader] string store)
        {
            if (store == null)
            {
                return UnprocessableEntity(new Response
                {
                    IsError = true,
                    Message = "Store portal não está presente no header.",
                });
            }

            var storeEntity = this._identityContext.Stores.FirstOrDefault(x => x.PortalUrl == store);
            if (storeEntity == null)
            {
                return NotFound(new Response
                {
                    IsError = true,
                    Message = "Loja não encontrada.",
                });
            }

            var response = await _catalogService.GetCatalogsResume(catalogQuery, storeEntity.StoreCode);

            if (response.filteredCatalogs.Any())
                return Ok(new Response { Message = "Catalogos", Payload = response.filteredCatalogs }.SetPagerInfo(response.pagerInfo));

            return Ok(new Response { Message = "Nenhum Catalogo encontrado", Payload = new object[] { }, });
        }

        [HttpGet, Route("{id}/details")]
        public async Task<IActionResult> GetCatalogDetailsById(int id)
        {
            var response = await _catalogService.GetCatalogDetailsById(id);

            if (response == null)
                return NotFound(new Response { Message = "Catalogo não encontrado", Payload = new object[] { } });
            else
                return Ok(new Response { Message = "Catalogo encontrado", Payload = response });
        }

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetCatalogById(int id)
        {
            CatalogProductsResponse response = await _catalogService.GetCatalogById(id);
            if (response == null)
                return NotFound(new Response { Message = "Catalogo não encontrado", });
            else
                return Ok(new Response { Message = "Catalogo encontrado", Payload = response });
        }

        [HttpGet, Route("{catalogId:int}/product")]
        public IActionResult GetCatalogProducts(int catalogId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 12)
        {
            var result = _catalogService.GetCatalogProductsById(catalogId, out PagerInfo pagerInfo, pageIndex, pageSize);
            if (result.Any())
                return Ok(new Response { Message = "Catalogo encontrado", Payload = result }.SetPagerInfo(pagerInfo));
            else
                return Ok(new Response { Message = "Nenhum Catalogo encontrado", Payload = new object[] { } });
        }

        [HttpGet, Route("{catalogId:int}/customer")]
        public async Task<ActionResult> GetCatalogUsers(int catalogId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var users = await this._context.CatalogCustomers.Include(x => x.Customer).Where(x => x.CatalogId == catalogId).Select(x => x.Customer).ToListAsync();
            if (users.Any())
                return Ok(new Response { Message = "Usuarios encontrados", Payload = users });

            return Ok(new Response { Message = "Nenhum Usuario encontrado", Payload = new object[] { } });
        }

        [HttpPost, Route("")]
        [HttpPost, Route("{id}")]
        public async Task<IActionResult> CreateCatalog([FromBody] CreateCatalogRequest request, [FromHeader] string store, [FromRoute] int? id = null)
        {
            try
            {
                Response res = await _catalogService.CreateCatalog(request, store, id);
                return StatusCode(res.Errors == null ? 200 : res.Errors.First().Code, res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response
                {
                    IsError = true,
                    Message = "Ocorreram problemas ao cadastrar o catalogo.",
                    Payload = new object[] { },
                });
            }
        }

        [HttpGet, Route("categories")]
        public async Task<IActionResult> Get()
        {
            var result = await _context.Categories.ToListAsync();
            var response = new Response { Payload = result };

            return Ok(response);
        }

        [HttpGet, Route("products")]
        public async Task<IActionResult> GetCatalogProducts([FromQuery] string term, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                Response res = await _catalogService.GetCatalogProducts(term, pageIndex, pageSize);

                return StatusCode(res.Errors == null ? 200 : res.Errors.First().Code, res);
            }
            catch (Exception e)
            {
                return StatusCode(500, new Response
                {
                    IsError = true,
                    Message = "Ocorreram problemas ao obter lista de produtos." + " " + e.Message,
                    Payload = new object[] { },
                });
            }
        }

        [HttpGet, Route("showcase")]
        public async Task<IActionResult> GetAllProducts([FromQuery] ShowcaseListingFilterRequest filter)
        {
            try
            {
                Response res = await _catalogService.GetAllProducts(filter);
                return StatusCode(res.Errors == null ? 200 : res.Errors.First().Code, res);
            }
            catch (Exception e)
            {
                return StatusCode(500, new Response
                {
                    IsError = true,
                    Message = "Ocorreram problemas ao obter lista de produtos da vitrine." + " " + e.Message,
                    Payload = new object[] { },
                });
            }
        }

        [HttpPost, Route("{ReceivedCatalogId:int}/{ReceivedCustomerId:int}")]
        public async Task<IActionResult> PostCatalogCustomerReceived([FromRoute] int ReceivedCatalogId, [FromRoute] int ReceivedCustomerId)
        {

            try
            {
                Response res = await _catalogService.PostCatalogCustomerReceived(ReceivedCatalogId, ReceivedCustomerId);

                return StatusCode(res.Errors == null ? 200 : res.Errors.First().Code, res);
            }
            catch (Exception e)
            {
                return StatusCode(500, new Response
                {
                    IsError = true,
                    Message = "Ocorreram problemas ao atualizar relação catálogo-consumidor.",
                    Payload = new object[] { },
                });
            }
        }

        [HttpGet, Route("{id}/instagram_list")]
        public async Task<IActionResult> GetCatalogsItensInfos(int id)
        {
            try
            {
                Response res = await _catalogService.GetCatalogsItensInfos(id);

                return StatusCode(res.Errors == null ? 200 : res.Errors.First().Code, res);
            }
            catch (Exception e)
            {
                return StatusCode(500, new Response
                {
                    IsError = true,
                    Message = "Ocorreram problemas ao retornar a lista do Instagram." + e.Message,
                    Payload = new object[] { },
                });
            }
        }
    }
}