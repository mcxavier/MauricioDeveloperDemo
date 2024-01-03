using Core.Vocabulary;
using FluentValidation;

namespace Core.Domains.Marketing.Commands.CreateNotificationCustomerWhenProductArrive
{
    public class CreateNotificationCustomerWhenProductArriveValidator : AbstractValidator<CreateNotificationCustomerWhenProductArriveCommand>
    {
        public CreateNotificationCustomerWhenProductArriveValidator()
        {
            this.Validate();
        }

        private void Validate()
        {
            RuleFor(x => x.CustomerName).NotEmpty().WithMessage(MarketingVocabulary.CustomerNameValidationMessage);
            RuleFor(x => x.CustomerEmail).EmailAddress().WithMessage(MarketingVocabulary.CustomerEmailValidationMessage);
            RuleFor(x => x.StockKeepingUnit).NotEmpty().WithMessage(MarketingVocabulary.StockKeepingUnitValidationMessage);
        }
    }
}