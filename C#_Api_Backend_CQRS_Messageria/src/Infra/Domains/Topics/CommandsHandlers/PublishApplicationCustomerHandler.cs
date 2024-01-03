using Core.Models.Core.Customers;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using Core.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Infra.QueryCommands.Commands.Topics
{
    class PublishApplicationCustomerHandler : IRequestHandler<PublishApplicationCustomer, Response>
    {
        private readonly CoreContext _context;
        private readonly ILogger<PublishApplicationCustomerHandler> _logger;
        private readonly SmartSalesIdentity _identity;

        public PublishApplicationCustomerHandler(CoreContext context, SmartSalesIdentity identity, ILogger<PublishApplicationCustomerHandler> logger)
        {
            _context = context;
            _identity = identity;
            _logger = logger;
        }

        public async Task<Response> Handle(PublishApplicationCustomer request, CancellationToken cancellationToken)
        {
            try
            {
                var dateOfBirth = request.dateOfBirth.Split("-");
                var birthDay = new DateTime(int.Parse(dateOfBirth[0]), int.Parse(dateOfBirth[1]), int.Parse(dateOfBirth[2]));

                var customer = _context.Customers.Where(x => x.Cpf == request.documentNumber.cpf).FirstOrDefault();

                if (customer != null)
                {
                    if (customer.ModifiedAt < request.updatedAt)
                    {
                        customer.BirthDay = birthDay.ToLocalTime();
                        customer.Cpf = request.documentNumber.cpf;
                        customer.Rg = request.documentNumber.rg;
                        customer.Email = request.email;
                        customer.Name = request.firstName + " " + request.lastName;
                        customer.Gender = request.gender;
                        customer.Phone = request.phone;
                        customer.ModifiedAt = request.updatedAt;
                        customer.ModifiedBy = "Linx.IO";                        

                        await _context.SaveChangesAsync();

                        return new Response("Cliente Atualizado", false);
                    }
                    else
                    {
                        return new Response("Dados do Cliente não atualizados porque a mensagem recebida é obsoleta.", true);
                    }
                }
                else
                {
                    customer = new Customer();
                    customer.CreatedAt = Utils.DateTimeBrazil.Now;
                    customer.ModifiedAt = request.updatedAt;
                    customer.ModifiedBy = "Linx.IO";
                    customer.CreatedBy = "Linx.IO";
                    customer.BirthDay = birthDay;
                    customer.Cpf = request.documentNumber.cpf;
                    customer.Rg = request.documentNumber.rg;
                    customer.Email = request.email;
                    customer.Name = request.firstName + " " + request.lastName;
                    customer.Gender = request.gender;
                    customer.Phone = request.phone;                    

                    await _context.Customers.AddAsync(customer);
                    await _context.SaveChangesAsync();

                    return new Response("Cliente cadastrado", false);
                }
            }
            catch (Exception ex)
            {
                return new Response("Cliente Inválido: " + ex.Message, true);
            }
        }
    }
}
