using Core.SharedKernel;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Infra.Domains.Topics.CommandsHandlers
{
    public class PublishApplicationVtexVariantListHandler : IRequestHandler<PublishApplicationVtexVariantList, Response>
    {
        private readonly CoreContext _context;
        private readonly ILogger<PublishApplicationVtexVariantListHandler> _logger;
        private readonly SmartSalesIdentity _identity;

        public PublishApplicationVtexVariantListHandler(CoreContext context, SmartSalesIdentity identity, ILogger<PublishApplicationVtexVariantListHandler> logger)
        {
            _context = context;
            _identity = identity;
            _logger = logger;
        }

        public async Task<Response> Handle(PublishApplicationVtexVariantList request, CancellationToken cancellationToken)
        {
            try
            {
                var variantList = new List<PublishApplicationVtexVariantList>();

                var variantIdList = (from vr in this._context.ProductVariations
                                     where !(from im in this._context.ProductImages select im.ProductVariationId).Contains(vr.Id)
                                     select new { vr.Id, vr.OriginId }).Take(200).ToList();


                foreach (var reg in variantIdList)
                {
                    variantList.Add(new PublishApplicationVtexVariantList()
                    {
                        skuId = reg.OriginId,
                        variationId = reg.Id.ToString()
                    });
                }

                return new Response("Sucess", false, variantList);
            }
            catch (Exception ex)
            {
                return new Response(ex.Message, true, new List<PublishApplicationVtexVariantList>());
            }
        }
    }
}
