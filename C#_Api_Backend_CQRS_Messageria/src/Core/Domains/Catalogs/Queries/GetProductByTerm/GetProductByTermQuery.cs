using MediatR;
using System.Collections.Generic;

namespace Core.QuerysCommands.Queries.Products.GetProductByTerm
{
    public class GetProductByTermQuery : IRequest<GetProductByTermQueryResponse>
    {
        public string Term { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool HasNext { get; set; } = false;
        public int? CategoryId { get; set; } = null;
        public List<int>? CategoriesIds { get; set; }

        public GetProductByTermQuery() { }

        public GetProductByTermQuery(string term)
        {
            Term = term;
        }
    }
}