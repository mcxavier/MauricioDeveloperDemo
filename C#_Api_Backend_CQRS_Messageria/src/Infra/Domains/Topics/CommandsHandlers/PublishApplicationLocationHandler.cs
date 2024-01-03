using Core.Models.Identity.Stores;
using Core.Models.Identity.Companies;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using Core.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Utils.Extensions;

namespace Infra.QueryCommands.Commands.Topics
{
    class PublishApplicationLocationHandler : IRequestHandler<PublishApplicationLocation, Response>
    {
        private readonly IdentityContext _context;
        private readonly ILogger<PublishApplicationLocationHandler> _logger;
        private readonly SmartSalesIdentity _identity;

        public PublishApplicationLocationHandler(IdentityContext context, SmartSalesIdentity identity, ILogger<PublishApplicationLocationHandler> logger)
        {
            _context = context;
            _identity = identity;
            _logger = logger;
        }

        public async Task<Response> Handle(PublishApplicationLocation request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Type.ToUpper().Equals("STORE"))
                {
                    Company company = _context.Companies.FirstOrDefault(x => x.Id == _identity.CurrentCompany);
                    var store = _context.Stores.FirstOrDefault(x => x.CompanyId == company.Id && x.OriginId == request.LocationId);
                    if (store == null)
                    {
                        store = new Store
                        {
                            Name = request.Name,
                            Description = request.Name,
                            OriginId = request.LocationId,
                            StoreCode = request.LocationCode.IsNullOrEmpty() ? request.LocationId : request.LocationCode,
                            IsActive = request.Enabled,
                            Phone = string.IsNullOrEmpty(request.Phone) ? (request.Address?.ContactPhone ?? request.Phone) : request.Phone,
                            Company = company,
                            CompanyId = company.Id,
                            Cnpj = request.DocumentNumber.Cnpj,
                            PortalUrl = request.DocumentNumber.Cnpj,
                            CreatedAt = Utils.DateTimeBrazil.Now,
                            ModifiedAt = request.UpdatedAt,
                            ModifiedBy = "Linx.IO",
                            CreatedBy = "Linx.IO"
                        };

                        await _context.Stores.AddAsync(store);

                        var storeAddress = new StoreAddress
                        {
                            Description = request.Name,
                            StoreId = store.Id,
                            ZipCode = request.Address.ZipCode,
                            StreetName = request.Address.AddressLine1,
                            Complement = request.Address.AddressLine2,
                            CityName = request.Address.City,
                            CountryName = request.Address.Country,
                            DistrictName = request.Address.Neighbourhood,
                            StreetNumber = request.Address.Number,
                            StateName = request.Address.State,
                            Store = store
                        };

                        await _context.StoreAddresses.AddAsync(storeAddress);

                        await _context.SaveChangesAsync();

                        return new Response("Loja cadastrada com sucesso.", false);
                    }
                    else
                    {
                        if (store.ModifiedAt < request.UpdatedAt)
                        {
                            store.Name = request.Name;
                            store.Description = request.Name;
                            store.IsActive = request.Enabled;
                            store.ModifiedAt = request.UpdatedAt;
                            store.ModifiedBy = "Linx.IO";

                            _context.Stores.Update(store);
                            await _context.SaveChangesAsync();

                            return new Response("Cadastro da Loja atualizado com sucesso.", false);
                        }
                        else
                        {
                            return new Response("Dados da loja descartados por ser informação já obsoleta.", true);
                        }
                    }
                }
                else
                {
                    return new Response("Tipo de Localização Irrelevante [" + request.Type.ToUpper() + "]", true);
                }
            }
            catch (System.Exception ex)
            {
                return new Response("Falha ao cadastrar loja: " + ex.Message, true);
            }
        }
    }
}
