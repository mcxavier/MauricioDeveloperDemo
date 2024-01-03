using Core.Dtos;
using PagedList.Core;

namespace Core.QuerysCommands.Queries.Products.GetProductByTerm
{
    public class GetProductByTermQueryResponse
    {
        public IPagedList<ProductResumeDto> Products { get; set; }
    }
}