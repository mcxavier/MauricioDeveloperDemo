using Core.SharedKernel;
using Infra.QueryCommands.Commands.Topics;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.Domains.Topics
{
    public class PublishApplicationCustomerExp : IRequest<Response>
    {
        public bool acceptsMarketing { get; set; }
        public IList<PublishApplicationCustomerAddress> addresses { get; set; }
        public IList<PublishApplicationCustomerAttribute> attributes { get; set; }
        public string createdAt { get; set; }
        public string customerId { get; set; }
        public string dateOfBirth { get; set; }
        public PublishApplicationCustomerDocumentNumber documentNumber { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string gender { get; set; }
        public string lastName { get; set; }
        public string mobilePhone { get; set; }
        public string notes { get; set; }
        public string phone { get; set; }
        public IList<PublishApplicationCustomerTag> tags { get; set; }
        public string type { get; set; }
        public string updatedAt { get; set; }
    }
}
