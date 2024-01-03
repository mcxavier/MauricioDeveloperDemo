using Core.Models.Core.Customers;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using Core.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;

namespace Infra.Domains.Topics.CommandsHandlers
{
    public class PublishApplicationCustomerExpHandler : IRequestHandler<PublishApplicationCustomerExp, Response>
    {
        private readonly CoreContext _context;
        private readonly ILogger<PublishApplicationCustomerExpHandler> _logger;
        private readonly SmartSalesIdentity _identity;

        public PublishApplicationCustomerExpHandler(CoreContext context, SmartSalesIdentity identity, ILogger<PublishApplicationCustomerExpHandler> logger)
        {
            _context = context;
            _identity = identity;
            _logger = logger;
        }

        public async Task<Response> Handle(PublishApplicationCustomerExp request, CancellationToken cancellationToken)
        {
            try
            {
                string lstCustomers = "";
                IList<Customer> customerList = _context.Customers.Where<Customer>(x => x.Id > 0).ToList();

                foreach (Customer customer in customerList)
                {
                    if (lstCustomers.Length > 1)
                    {
                        lstCustomers = lstCustomers + ";";
                    }

                    lstCustomers = lstCustomers + JsonConvert.SerializeObject(customer);
                }

                return new Response(lstCustomers, true, customerList);
            }
            catch (Exception ex)
            {
                return new Response("Customer Inválido: " + ex.Message, true, request);
            }
        }
    }
}
