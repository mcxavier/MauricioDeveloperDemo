using MediatR;
using System;
using System.Collections.Generic;
using Core.SharedKernel;
using Newtonsoft.Json;
using Core.Models.Core.Ordering;

namespace Infra.QueryCommands.Commands.Topics
{
    public class PublishApplicationOrderStatus : IRequest<Response>
    {
        public string OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }
}
