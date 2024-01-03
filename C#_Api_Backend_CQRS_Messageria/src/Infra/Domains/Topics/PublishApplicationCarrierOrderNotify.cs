using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Core.SharedKernel;
using Newtonsoft.Json;
using Infra.QueryCommands.Commands.Topics;

namespace Infra.Domains.Topics
{
    public class PublishApplicationCarrierOrderNotify : IRequest<Response>
    {
        public string BrandId { get; set; }
        public string ChannelType { get; set; }
        public PublishApplicationOrder_Customer Customer { get; set; }
        public string FulfillmentId { get; set; }
        public List<string> Invoices { get; set; }
        public PublishApplicationLocation Location { get; set; }
        public string OrderId { get; set; }
        public PublishApplicationOrder_Shipment Shipment { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}