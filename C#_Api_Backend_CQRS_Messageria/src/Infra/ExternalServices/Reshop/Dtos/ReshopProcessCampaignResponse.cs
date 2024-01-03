namespace Infra.ExternalServices.Reshop.Dtos
{
    public class ReshopProcessCampaignResponse
    {
        public bool Resultado { get; set; }
        public string Mensagem { get; set; }
        public string Nsu { get; set; }
        public decimal PontosCreditados { get; set; }
        public decimal ValorCreditado { get; set; }
        public decimal PontosDebitados { get; set; }
        public decimal ValorDebitado { get; set; }
        public string MensagemCampanha { get; set; }
        public object Recibo { get; set; }
        public int IdTransacao { get; set; }
        public object PreSaleId { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
        public bool IsException { get; set; }
    }
}
