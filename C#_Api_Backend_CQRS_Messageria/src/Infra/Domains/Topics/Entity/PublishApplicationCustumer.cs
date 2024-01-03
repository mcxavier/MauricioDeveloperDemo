using MediatR;
using System;
using System.Collections.Generic;
using Core.SharedKernel;

namespace Infra.QueryCommands.Commands.Topics
{
    public class PublishApplicationCustomer : IRequest<Response>
    {
        public bool acceptsMarketing { get; set; }
        public IList<PublishApplicationCustomerAddress> addresses { get; set; }
        public IList<PublishApplicationCustomerAttribute> attributes { get; set; }
        public DateTime createdAt { get; set; }
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
        public DateTime updatedAt { get; set; }
    }

    public class PublishApplicationCustomerAddress
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string addressId { get; set; }
        public string addressName { get; set; }
        public string city { get; set; }
        public string contactName { get; set; }
        public string contactPhone { get; set; }
        public string country { get; set; }
        public bool @default { get; set; }
        public int latitude { get; set; }
        public int longitude { get; set; }
        public string neighbourhood { get; set; }
        public string notes { get; set; }
        public string number { get; set; }
        public string state { get; set; }
        public string zipCode { get; set; }
    }

    public class PublishApplicationCustomerAttribute
    {
        public string id { get; set; }
        public string name { get; set; }
        public PublishApplicationCustomerAttributesData Data { get; set; }
    }

    public class PublishApplicationCustomerAttributesData
    {
        public string id { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }

    public class PublishApplicationCustomerDocumentNumber
    {
        public string cpf { get; set; }
        public string rg { get; set; }
    }

    public class PublishApplicationCustomerTag
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
