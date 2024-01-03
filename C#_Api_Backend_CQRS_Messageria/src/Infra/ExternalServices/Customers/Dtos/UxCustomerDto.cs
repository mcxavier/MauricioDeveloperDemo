using System;

namespace Infra.ExternalServices.Customers.Dtos
{

    public class UxCustomerDto
    {

        public string StoreCode { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Document { get; set; }

        public DateTime? BirthDay { get; set; }

        public string ContactNumber { get; set; }

        public string Telephone { get; set; }

        public string Rg { get; set; }

    }

}