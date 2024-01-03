using Api.App_Infra;
using Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domains.Catalogs.Repositories;
using Core.Models.Core.Products;
using Core.QuerysCommands.Queries.Products.GetProductByTerm;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using MediatR;
using MoreLinq.Extensions;
using Core.SharedKernel;
using Core.Repositories;

namespace Api.Controllers
{
    [ServiceFilter(typeof(SmartSalesAuthorizeAttribute))]
    [Route("api/v1/[controller]"), ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductsRepository _productRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IMediator _mediator;
        private readonly CoreContext _context;
        private readonly SmartSalesIdentity _identity;

        public ProductController(CoreContext context,
                IProductsRepository productRepository,
                IStockRepository stockRepository,
                SmartSalesIdentity identity,
                IMediator mediator)
        {
            this._context = context;
            this._productRepository = productRepository;
            this._stockRepository = stockRepository;
            this._identity = identity;
            this._mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var products = await this._productRepository.GetProductsPaginated(pageIndex, pageSize);

            if (products.Any())
            {
                return Ok(new Response
                {
                    Message = "Produtos encontrados",
                    Payload = products.ToList()
                }.SetPagerInfo(products));
            }

            return Ok(new Response
            {
                Message = "Nenhum Produto encontrado",
                Payload = new object[] { },
            });
        }

        [HttpGet, Route("{sku}/stocks")]
        public async Task<ActionResult> GetStocksBySku(string sku)
        {
            try
            {
                //TODO: Implementar a nova flag de estoque minimo pra venda
                var stock = await this._stockRepository.GetProductStockAsync(sku);
                if (stock == null)
                    return Ok(new Response { Message = "Nenhum produto encontrado", Payload = null });

                return Ok(new Response { Message = "Produto encontrado", Payload = stock });
            }
            catch (System.Exception ex)
            {
                return Ok(new Response { Message = ex.Message, Payload = null, IsError = true });
            }
        }

        [HttpGet, Route("{id}")]
        public async Task<ActionResult> GetProductById(int id)
        {
            try
            {
                var product = _productRepository.GetProductById(id);
                if (product == null)
                    return NotFound(new Response { Message = "Nenhum produto encontrado", Payload = new object[] { }, });

                var variations = product.Variations ?? new List<ProductVariation>();
                product.Variations = null;

                var view = new ProductListingQueryModel
                {
                    Product = product,
                    Variations = variations,
                    Specifications = variations.SelectMany(x => x.Specifications)
                                               .Where(x => x.TypeId == ProductSpecificationType.Color.Id || x.TypeId == ProductSpecificationType.Size.Id)
                                               .Select(x => new ProductSpecificationDto
                                               {
                                                   TypeId = x.TypeId,
                                                   Value = x.Value,
                                                   Name = x.Name,
                                                   Description = x.Description,
                                               }).DistinctBy(x => x.Value).ToList(),
                };

                //TODO: Implementar a nova flag de estoque minimo pra venda
                var stocks = await this._stockRepository.GetProductsStockAsync(view.Specifications.Select(x => x.Value).ToArray());
                foreach (var sku in view.Specifications)
                {
                    sku.Stock = stocks?.FirstOrDefault(x=>x.StockKeepingUnit == sku.Value)?.Units ?? 0;
                }

                return Ok(new Response { Message = "Produto encontrado", Payload = view });
            }
            catch (System.Exception ex)
            {
                return Ok(new Response { Message = ex.Message, Payload = null, IsError = true });
            }
        }

        [HttpGet, Route("search")]
        public async Task<IActionResult> GetProductByTerm([FromQuery] GetProductByTermQuery query)
        {
            var response = await this._mediator.Send(query);
            if (!response.Products.Any())
            {
                return Ok(new Response("Nenhum produto encontrado", false, new object[] { }));
            }

            return Ok(new Response
            {
                Message = "sku encontrado com sucesso",
                Payload = response.Products.ToList()
            }.SetPagerInfo(response.Products));
        }

        [HttpGet, Route("categories")]
        public async Task<ActionResult> GetCategories()
        {
            var categories = _context.Categories.ToList();
            if (categories == null)
            {
                return NotFound(new Response { Message = "Nenhuma categoria encontrada" });
            }

            return Ok(new Response { Message = "Categorias encontradas com sucesso.", Payload = categories });
        }
    }
}