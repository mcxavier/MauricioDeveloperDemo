using Core.SeedWork;

namespace Core.Models.Core.Payments
{
    public class PaymentTransactionSupplierCodeType : Enumeration
    {
        public string Description { get; set; }

        public PaymentTransactionSupplierCodeType(int id, string name, string description) : base(id, name)
        {
            Description = description;
        }

        public static readonly PaymentTransactionSupplierCodeType NotAllowed = new PaymentTransactionSupplierCodeType(1, nameof(NotAllowed), "Transação não permitida para o emissor.");

        public static readonly PaymentTransactionSupplierCodeType ErrorOnDataReported = new PaymentTransactionSupplierCodeType(2, nameof(ErrorOnDataReported), "Erro nos dados reportados.");

        public static readonly PaymentTransactionSupplierCodeType ErrorOnCredentials = new PaymentTransactionSupplierCodeType(3, nameof(ErrorOnCredentials), "Problemas no credenciamento. Entre em contato com nosso suporte.");

        public static readonly PaymentTransactionSupplierCodeType NotAuthorized = new PaymentTransactionSupplierCodeType(4, nameof(NotAuthorized), "Não Autorizado. O portador do cartão deve entrar em contato com o emissor do cartão.");

        public static readonly PaymentTransactionSupplierCodeType TransactionNotAllowed = new PaymentTransactionSupplierCodeType(5, nameof(TransactionNotAllowed), "Transação não permitida. Entre em contato com nosso suporte.");

        public static readonly PaymentTransactionSupplierCodeType ComunicationFailure = new PaymentTransactionSupplierCodeType(6, nameof(ComunicationFailure), "Falha de Comunicação. Tente novamente.");

        public static readonly PaymentTransactionSupplierCodeType ExpiredCard = new PaymentTransactionSupplierCodeType(7, nameof(ExpiredCard), "Cartão Expirado. O portador do cartão deve entrar em contato com o emissor do cartão.");

        public static readonly PaymentTransactionSupplierCodeType CardLimitExceeded = new PaymentTransactionSupplierCodeType(8, nameof(CardLimitExceeded), "Cartão Sem Limite. O portador do cartão deve entrar em contato com o emissor do cartão.");

        public static readonly PaymentTransactionSupplierCodeType CardProblems = new PaymentTransactionSupplierCodeType(9, nameof(CardProblems), "Problemas no Cartão. O portador do cartão deve entrar em contato com o emissor do cartão.");

        public static readonly PaymentTransactionSupplierCodeType NotAuthorizedTryAgain = new PaymentTransactionSupplierCodeType(10, nameof(NotAuthorizedTryAgain), "Não Autorizado. Tente novamente.");

        public static readonly PaymentTransactionSupplierCodeType NotAuthorizedNonExistentCard = new PaymentTransactionSupplierCodeType(11, nameof(NotAuthorizedNonExistentCard), "Não Autorizado. Cartão não existente.");

        public static readonly PaymentTransactionSupplierCodeType NotAuthorizedNoReason = new PaymentTransactionSupplierCodeType(12, nameof(NotAuthorizedNoReason), "Transação não autorizada.");

        public static readonly PaymentTransactionSupplierCodeType NotAuthorizedIdentifiedRisk = new PaymentTransactionSupplierCodeType(13, nameof(NotAuthorizedIdentifiedRisk), "Não Autorizado. Risco identificado pelo emissor.");

        public static readonly PaymentTransactionSupplierCodeType TransactionNotFound = new PaymentTransactionSupplierCodeType(14, nameof(TransactionNotFound), "Transação não encontrada.");

        public static readonly PaymentTransactionSupplierCodeType NotAuthorizedInvalidSecurityNumber = new PaymentTransactionSupplierCodeType(16, nameof(NotAuthorizedInvalidSecurityNumber), "Não Autorizado. Código de Segurança Inválido.");

        public static readonly PaymentTransactionSupplierCodeType InvalidCardNumber = new PaymentTransactionSupplierCodeType(17, nameof(InvalidCardNumber), "Número do Cartão Inválido");

        public static readonly PaymentTransactionSupplierCodeType ProcessingError = new PaymentTransactionSupplierCodeType(18, nameof(ProcessingError), "Erro no Processamento. Tente novamente.");

        public static readonly PaymentTransactionSupplierCodeType TransactionSendBefore = new PaymentTransactionSupplierCodeType(19, nameof(TransactionSendBefore), "Transação enviada anteriormente");

        public static readonly PaymentTransactionSupplierCodeType NotAuthorizedCallSupport = new PaymentTransactionSupplierCodeType(20, nameof(NotAuthorizedCallSupport), "Transação não autorizada. Entre em contato com nosso suporte.");

    }

    public enum PaymentTransactionSupplierCodeTypeEnum
    {
        NotAllowed = 1,
        ErrorOnDataReported = 2,
        ErrorOnCredentials = 3,
        NotAuthorized = 4,
        TransactionNotAllowed = 5,
        ComunicationFailure = 6,
        ExpiredCard = 7,
        CardLimitExceeded = 8,
        CardProblems = 9,
        NotAuthorizedTryAgain = 10,
        NotAuthorizedNonExistentCard = 11,
        NotAuthorizedNoReason = 12,
        NotAuthorizedIdentifiedRisk = 13,
        TransactionNotFound = 14,
        NotAuthorizedInvalidSecurityNumber = 16,
        InvalidCardNumber = 17,
        ProcessingError = 18,
        TransactionSendBefore = 19,
        NotAuthorizedCallSupport = 20,
    }
}