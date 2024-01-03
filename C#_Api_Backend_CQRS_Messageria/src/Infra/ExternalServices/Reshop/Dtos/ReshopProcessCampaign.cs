using System;
using System.Collections.Generic;

namespace Infra.ExternalServices.Reshop.Dtos
{
    public class ReshopProcessCampaign
    {
        public string CodigoLoja { get; set; }
        public DateTime DataHora { get; set; }
        public DateTime DataVenda { get; set; }
        public int TipoVenda { get; set; }
        public string Nsu { get; set; }
        public decimal QtdeTotal { get; set; }
        public decimal? ValorBruto { get; set; }
        public decimal? ValorDescontos { get; set; }
        public string TransacaoAssociada { get; set; }
        public decimal? ValorDescontoUnico { get; set; }
        public decimal? ValorAcrescimos { get; set; }
        public decimal? ValorLiquido { get; set; }
        public string NumeroOperacao { get; set; }
        public string SenhaAutorizacao { get; set; }
        public string ProtocoloOffline { get; set; }
        public bool TokenIn2Char { get; set; }
        public int SourceApplication { get; set; }
        public List<ItemProcessCampaign> Itens { get; set; }
        public List<Pagamento> Pagamentos { get; set; }
        public bool? Result { get; set; }
        public string Message { get; set; }
        public bool? IsException { get; set; }
        public bool? Offline { get; set; }
        public MessagesValidation4 MessagesValidation { get; set; }
    }
    public class ItemProcessCampaign
    {
        public string Item { get; set; }
        //public string CodigoVendedor { get; set; }
        public string Ncm { get; set; }
        public string CodigoProduto { get; set; }
        public string CodigoSku { get; set; }
        public decimal Qtde { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal ValorLiquido { get; set; }

    }
    public class Pagamento
    {
        public int NumeroPagamento { get; set; }
        public string Codigo { get; set; }
        public int Tipo { get; set; }
        public string BinCartao { get; set; }
        public DateTime Vencimento { get; set; }
        public decimal Valor { get; set; }
        public int IdVoucher { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
        public bool IsException { get; set; }
        public bool Offline { get; set; }
        public MessagesValidation3 MessagesValidation { get; set; }

    }

    public class ItensDevolvido
    {
        // public string Item { get; set; }
        public string CodigoVendedor { get; set; }
        public string Ncm { get; set; }
        public string CodigoProduto { get; set; }
        public string CodigoSku { get; set; }
        //public string EanSku { get; set; }
        public int Qtde { get; set; }
        public int PrecoUnitario { get; set; }
        public int Desconto { get; set; }
        public int ValorLiquido { get; set; }
        public int ValorDescontoUnico { get; set; }
        public bool ItemEcommerce { get; set; }
        public bool ItemServico { get; set; }
        public bool ItemPresente { get; set; }
        public int QtdeEcommerce { get; set; }
        public int QtdeServico { get; set; }
        public int PercentualRepresentacao { get; set; }
        public int ValorEfetivo { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
        public bool IsException { get; set; }
        public bool Offline { get; set; }
        public MessagesValidation2 MessagesValidation { get; set; }

    }

    public class MessagesValidation
    {

    }

    public class MessagesValidation2
    {

    }

    public class MessagesValidation3
    {

    }

    public class MessagesValidation4
    {

    }
}
