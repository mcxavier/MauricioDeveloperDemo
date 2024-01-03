using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.ExternalServices.Reshop.Dtos
{
    public class ReshopConfirmsCampaignResponse
    {
        public bool Resultado { get; set; }
        public string Mensagem { get; set; }
        public string Nsu { get; set; }
        public int QtdeCampanhasAtivadas { get; set; }
        public int QtdeCampanhasPromocode { get; set; }
        public int QtdeCampanhasConflito { get; set; }
        public int QtdeCampanhasBrindeAdicionar { get; set; }
        public double TotalDesconto { get; set; }
        public object ValidaCampanhaErro { get; set; }
        public string Log { get; set; }
        public bool AtivacaoOffline { get; set; }
        public List<CampanhasAtivada> CampanhasAtivadas { get; set; }
        public List<object> CampanhasConflito { get; set; }
        public List<object> CampanhasBrindeAdicionar { get; set; }
        public List<object> CamposCadastro { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
        public bool IsException { get; set; }
    }

    public class CampanhasAtivada
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int QtdeAtivacoes { get; set; }
        public string NomeCampanha { get; set; }
        public string DescricaoCampanha { get; set; }
        public string DescricaoComercial { get; set; }
        public int OrdemCampanha { get; set; }
        public object Foto { get; set; }
        public string Recibo { get; set; }
        public double ValorDescontoSubtotal { get; set; }
        public double PontosFidelidadeAdicionar { get; set; }
        public object NomeCampanhaDestino { get; set; }
        public double PercentualDescontoDestino { get; set; }
        public double ValorDescontoDestino { get; set; }
        public DateTime? Vencimento { get; set; }
        public bool Promocode { get; set; }
        public bool Exclusiva { get; set; }
        public bool Restrita { get; set; }
        public bool InibeResgate { get; set; }
        public bool SolicitaTokenApp { get; set; }
        public int ConflictBehavior { get; set; }
        public int IdConflito { get; set; }
        public object BenefitPromocode { get; set; }
        public int QuantidadeBrindes { get; set; }
        public double ValorDescontoCompleto { get; set; }
        public bool DescontoNosItens { get; set; }
        public List<object> BeneficiosProduto { get; set; }
        public List<object> RegrasPagamento { get; set; }
        public List<Itens> Itens { get; set; }
        public object Devolucoes { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
        public bool IsException { get; set; }

    }

    public class Itens
    {
        public string Item { get; set; }
        public double ValorUnitario { get; set; }
        public double ValorDescontoItem { get; set; }
        public object EanSku { get; set; }
        public string CodigoProduto { get; set; }
        public string CodigoSku { get; set; }
        public double Qtde { get; set; }
        public double ValorOriginal { get; set; }
        public double DescontoOriginal { get; set; }
        public double ValorTotalDescontoItem { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
        public bool IsException { get; set; }

    }
}
