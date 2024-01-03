namespace PagarMeApi
{
    public partial class Configuration
    {
        public static string BaseUri = "https://api.pagar.me/core/v5";
        public static string BasicAuthUserName = "";
        public static string BasicAuthPassword = "";
    }

    public class LinxIOPagarMeClientConfig
    {
        public string BasicAuthUserName { get; set; }
        public string BasicAuthPassword { get; set; }
        public string RecipientIdSmartSales { get; set; }
    }

    public class LinxIOPagarMeStoreConfig
    {
        public string Type { get; set; }
        public string CreditCard { get; set; }
        public string DebitCard { get; set; }
        public string PaymentSlip { get; set; }
        public string Pix { get; set; }
        public string BasicAuthPassword { get; set; }
        public string RecipientIdLojista { get; set; }
        public string TransactionTypeCreditCard { get; set; }
        public string ChargeRemainderFee { get; set; }
        public string ChargeProcessingFee { get; set; }
        public string Liable { get; set; }
    }
}