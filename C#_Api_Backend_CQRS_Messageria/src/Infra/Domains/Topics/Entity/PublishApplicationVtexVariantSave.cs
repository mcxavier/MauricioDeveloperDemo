using Core.SharedKernel;
using MediatR;

namespace Infra.Domains.Topics
{
    public class PublishApplicationVtexVariantSave : IRequest<Response>
    {
        public string name { get; set; }
        public string urlImage { get; set; }
        public bool isPrincipal { get; set; }
        public string productVariationId { get; set; }
    }
}
