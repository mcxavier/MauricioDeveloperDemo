using System;
using System.Collections.Generic;

namespace Infra.ExternalServices.Reshop.Dtos
{
    public class ReshopQueryCampaign
    {
        public string CodigoLoja { get; set; }
        public DateTime DataHora { get; set; }
        public decimal QtdeTotal { get; set; }
        public double ValorBruto { get; set; }
        public double ValorLiquido { get; set; }
        public string TransacaoAssociada { get; set; }
        public bool SomenteRemarcacao { get; set; }
        public List<string> Promocodes { get; set; }
        public List<Iten> Itens { get; set; }
        public List<object> ItensDevolvidos { get; set; }
        public List<object> Pagamentos { get; set; }
        public List<object> CampanhasOpcionais { get; set; }
        public List<object> CampanhasAtivadas { get; set; }
    }

    public class Iten
    {
        public string Item { get; set; }
        public string Ncm { get; set; }
        public string CodigoProduto { get; set; }
        public string CodigoSku { get; set; }
        public decimal Qtde { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal ValorLiquido { get; set; }
    }
}
