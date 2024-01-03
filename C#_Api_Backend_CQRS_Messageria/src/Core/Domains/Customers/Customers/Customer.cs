using System;
using System.Collections.Generic;
using Core.SeedWork;
using Core.SharedKernel;

namespace Core.Models.Core.Customers
{
    public class Customer : EntityWithMetadata<int>, IAggregateRoot, IStoreReferenced
    {
        public string StoreCode { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public DateTime? BirthDay { get; set; }
        public IList<CustomersOrderHistory>? OrderHistory { get; set; }
    }
}