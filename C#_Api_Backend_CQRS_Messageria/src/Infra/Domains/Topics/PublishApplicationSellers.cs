using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Core.SharedKernel;
using Newtonsoft.Json;
using Core.SeedWork;

namespace Infra.QueryCommands.Commands.Topics
{
    public class PublishApplicationSellers : EntityWithMetadata<int>, IRequest<Response>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }
        public DateTime? BirthDay { get; set; }
        public int OriginId { get; set; }
    }
}
