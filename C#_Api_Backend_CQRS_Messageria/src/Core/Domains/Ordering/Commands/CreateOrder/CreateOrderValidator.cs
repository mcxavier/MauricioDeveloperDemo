using Core.Domains.Catalogs.Repositories;
using Core.Domains.Customers.Repositories;
using FluentValidation;

namespace Core.QuerysCommands.Commands.Orders.CreateOrder
{
    public class CreateOrderValidator : AbstractValidator<CreadeOrderCommand>
    {

        private readonly ICatalogRepository _catalogRepository;
        private readonly ICustomersRepository _customersRepository;

        public CreateOrderValidator(ICatalogRepository catalogRepository, ICustomersRepository customersRepository)
        {
            _catalogRepository = catalogRepository;
            _customersRepository = customersRepository;
            this.Validate();
        }

        private void Validate()
        {
            RuleFor(x => x.CatalogId).Must(CatalogExists).WithMessage("Catalogo não existente");
            RuleFor(x => x.Installments).GreaterThan((byte)0).WithMessage("A quantidade de parcelas deverá ser preenchida");
            RuleFor(x => x.Items).Must(x => x.Count > 0).WithMessage("Os itens precisam ser preenchidos");
            RuleFor(x => x.Address.ZipCode).Length(8).WithMessage("Cep precisa conter 8 dígitos");
            RuleFor(x => x.User.Cpf).Length(11).WithMessage("Cep precisa conter 11 dígitos");
            RuleFor(x => x.User.Email).EmailAddress().WithMessage("Email inválido");
            RuleFor(x => x.User.Email).Must(UserExistsByEmail).WithMessage("Usuario não existente");
        }

        private bool CatalogExists(int id)
        {
            var catalog = _catalogRepository.GetCatalogById(id).Result;
            return catalog != null;
        }

        private bool UserExistsByEmail(string email)
        {
            var catalog = this._customersRepository.GetCustomerByEmail(email).Result;
            return catalog != null;
        }
    }
}