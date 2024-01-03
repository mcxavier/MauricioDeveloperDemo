using Core.SharedKernel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.Domains.Topics
{
    public class PublishApplicationVtexVariantList : IRequest<Response>
    {
        public string skuId { get; set; }
        public string variationId { get; set; }

    }
}
