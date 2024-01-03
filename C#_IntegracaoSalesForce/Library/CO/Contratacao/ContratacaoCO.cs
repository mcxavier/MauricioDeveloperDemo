using PortalParceiroLibrary.BO.Salesforce;
using PortalParceiroLibrary.CO.Salesforce;
using static PortalParceiroLibrary.CO.Salesforce.SincronizacaoSalesforceCO;

namespace PortalParceiroLibrary.CO.Contratacao
{
    public class ContratacaoCO : IContratacaoCO
    {
 

        public ContratacaoCO()
        {
 
        }

        public int AlterarIdSalesforce(BancoDadosBO db, int idContratacao, string idFromSalesforce)
        {
            return _contratacaoDAO.AlterarIdSalesforce(db, idContratacao, idFromSalesforce);
        }


        private void AtualizarCobrancasDaContratacaoNaSuperLogica(BancoDadosBO db, ContratacaoBO contratacao, decimal valorContratacaoAtual)
        {

            SincronizacaoSalesforceCO _sincronizacaoSalesforceCO = new SincronizacaoSalesforceCO();
            ContratoSalesforceBO usuarioSalesforce = new ContratoSalesforceBO()
            {
                IdClass = contratacao.IdContratacao,
                Id = _contratacaoDAO.GetIdSalesforce(db, contratacao.IdContratacao),
                accountId = contratacao.IdCliente.ToString(),
                Description = contratacao.DescricaoDoDescontoDaImplantacao,
                Plano__c = contratacao.IdPeriodoContratacao.ToString(),
                Produto__c = contratacao.CodigoInternoProduto.ToString(),
                Valor_Assinatura__c = contratacao.ValorAssinatura
            };

            _sincronizacaoSalesforceCO.AddRegistroSincronismo(db, IdentClass.Contrato, contratacao.IdContratacao, IdentAction.Update, DateTime.Now,
                JsonSerializer.Serialize<ContratoSalesforceBO>(usuarioSalesforce));

        }


        private ContratacaoBO GetContratacaoPelaAtivacao(BancoDadosBO db, ClienteParaContratacaoBO cliente)
        {
            SincronizacaoSalesforceCO _sincronizacaoSalesforceCO = new SincronizacaoSalesforceCO();
            ContratoSalesforceBO usuarioSalesforce = new ContratoSalesforceBO()
            {
                IdClass = contratacao.IdContratacao,
                Id = _contratacaoDAO.GetIdSalesforce(db, contratacao.IdContratacao),
                accountId = contratacao.IdCliente.ToString(),
                Description = contratacao.DescricaoDoDescontoDaImplantacao,
                Plano__c = contratacao.IdPeriodoContratacao.ToString(),
                Produto__c = contratacao.CodigoInternoProduto.ToString(),
                Valor_Assinatura__c = contratacao.ValorAssinatura
            };

            _sincronizacaoSalesforceCO.AddRegistroSincronismo(db, IdentClass.Contrato, contratacao.IdContratacao, IdentAction.Update, DateTime.Now,
                JsonSerializer.Serialize<ContratoSalesforceBO>(usuarioSalesforce));

            return contratacao;

        }


        private void RecalcularTotaisDosPeriodos(BancoDadosBO db, ContratacaoBO contratacao, IEnumerable<IGrouping<int, ComponenteContratacaoBO>> componentesAgrupados)
        {
            SincronizacaoSalesforceCO _sincronizacaoSalesforceCO = new SincronizacaoSalesforceCO();
            ContratoSalesforceBO usuarioSalesforce = new ContratoSalesforceBO()
            {
                IdClass = contratacao.IdContratacao,
                Id = _contratacaoDAO.GetIdSalesforce(db, contratacao.IdContratacao),
                accountId = contratacao.IdCliente.ToString(),
                Description = contratacao.DescricaoDoDescontoDaImplantacao,
                Plano__c = contratacao.IdPeriodoContratacao.ToString(),
                Produto__c = contratacao.CodigoInternoProduto.ToString(),
                Valor_Assinatura__c = contratacao.ValorAssinatura
            };

            _sincronizacaoSalesforceCO.AddRegistroSincronismo(db, IdentClass.Contrato, contratacao.IdContratacao, IdentAction.Update, DateTime.Now,
                JsonSerializer.Serialize<ContratoSalesforceBO>(usuarioSalesforce));
        }

        private void RecalcularUpsell(BancoDadosBO db, ContratacaoBO contratacao, ContratacaoBO contratacaoAtivada, ICollection<PrecoComponenteBO> precos)
        {
            SincronizacaoSalesforceCO _sincronizacaoSalesforceCO = new SincronizacaoSalesforceCO();
            ContratoSalesforceBO usuarioSalesforce = new ContratoSalesforceBO()
            {
                IdClass = contratacao.IdContratacao,
                Id = _contratacaoDAO.GetIdSalesforce(db, contratacao.IdContratacao),
                accountId = contratacao.IdCliente.ToString(),
                Description = contratacao.DescricaoDoDescontoDaImplantacao,
                Plano__c = contratacao.IdPeriodoContratacao.ToString(),
                Produto__c = contratacao.CodigoInternoProduto.ToString(),
                Valor_Assinatura__c = contratacao.ValorAssinatura
            };

            _sincronizacaoSalesforceCO.AddRegistroSincronismo(db, IdentClass.Contrato, contratacao.IdContratacao, IdentAction.Update, DateTime.Now,
                JsonSerializer.Serialize<ContratoSalesforceBO>(usuarioSalesforce));
        }

        private void RecalcularUpsellPorAlteracaoNoValorAdicionalDaAssinatura(BancoDadosBO db, ContratacaoBO contratacao, ContratacaoBO contratacaoAtivada)
        {
            SincronizacaoSalesforceCO _sincronizacaoSalesforceCO = new SincronizacaoSalesforceCO();
            ContratoSalesforceBO usuarioSalesforce = new ContratoSalesforceBO()
            {
                IdClass = contratacao.IdContratacao,
                Id = _contratacaoDAO.GetIdSalesforce(db, contratacao.IdContratacao),
                accountId = contratacao.IdCliente.ToString(),
                Description = contratacao.DescricaoDoDescontoDaImplantacao,
                Plano__c = contratacao.IdPeriodoContratacao.ToString(),
                Produto__c = contratacao.CodigoInternoProduto.ToString(),
                Valor_Assinatura__c = contratacao.ValorAssinatura
            };

            _sincronizacaoSalesforceCO.AddRegistroSincronismo(db, IdentClass.Contrato, contratacao.IdContratacao, IdentAction.Update, DateTime.Now,
                JsonSerializer.Serialize<ContratoSalesforceBO>(usuarioSalesforce));
        }

 
    }
}