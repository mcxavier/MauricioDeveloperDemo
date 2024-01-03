using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domains.Catalogs.Filters;
using Core.Domains.Catalogs.Repositories;
using Core.QuerysCommands.Queries.Products.GetProductByTerm;
using MediatR;

namespace Infra.QueryCommands.QueriesHandlers.Products
{
    public class GetProductByTermQueryHandler : IRequestHandler<GetProductByTermQuery, GetProductByTermQueryResponse>
    {
        private readonly IProductsRepository _productsRepository;

        public GetProductByTermQueryHandler(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task<GetProductByTermQueryResponse> Handle(GetProductByTermQuery request, CancellationToken cancellationToken)
        {
            var repositoryFilter = new ProductFilter
            {
                Term = request.Term,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Filters = new List<string>(),
                CategoryId = request.CategoryId,
                CategoriesIds = request.CategoriesIds
            };

            if (request.HasNext) repositoryFilter.Filters.Add("hasNext");

            var list = await _productsRepository.GetProductResumeByTerm(repositoryFilter);

            return new GetProductByTermQueryResponse
            {
                Products = list
            };
        }
    }
}