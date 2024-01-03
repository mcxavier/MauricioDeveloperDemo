using PortalParceiroLibrary.BO.Salesforce;
using PortalParceiroLibrary.CO.Salesforce;
using static PortalParceiroLibrary.CO.Salesforce.SincronizacaoSalesforceCO;

namespace PortalParceiroLibrary.CO
{
    public class ClienteCO : IClienteCO
    {

        public ClienteCO(
        {
        }

         public static void AlterarCliente(BancoDadosBO db, int idCliente, int idParceiro, string cnpj, string ie, string nome, string nomeFantasia,
            string logradouro, string numeroEndereco, string bairro, string complemento, string cep, int? idCidade, string foneDdd,
            string foneNumero, string email, int idUsuarioAlteracao, int idSegmento, TipoCliente tipoCliente,
             int? idParceiroPosVenda, int? idSegmentoOutro, string responsavelNome, string responsavelCpf, string responsavelFoneDDD, string responsavelFoneNumero, bool atendimentoRealizadoPelaHiper,
             bool permiteAlterarDadosResponsavel, DateTime? responsavelDataNascimento = null, bool responsavelPeloContratoEhAHiper = false, bool permiteDefinirResponsavelPeloContrato = false,
             short? idTipoDeResponsavel = null, string cnae = "", decimal? percentualDescontoDiferenciado = null)
        {

            SincronizacaoSalesforceCO _sincronizacaoSalesforceCO = new SincronizacaoSalesforceCO();
            ClienteSalesforceBO clienteSalesforce = new ClienteSalesforceBO()
            {
                IdClass = idCliente,
                Description = email,
                Name = nome,
                Id = ClienteDAO.GetIdSalesforce(db, idCliente),
                Atendimento_Hiper__c = idParceiro.ToString(),
                Bairro__c = bairro,
                CNAE__c = cnae,
                CNPJ__c = cnpj,
                Complemento__c = complemento,
                DDD__c = foneDdd,
                E_mail_c__c = email,
                ID_Cliente__c = idCliente.ToString(),
                Inscricao_Estadual__c = ie,
                Nome_Fantasia__c = nomeFantasia,
                Numero__c = numeroEndereco,
                Phone = foneNumero,
                Prius__c = "",
                Segmento__c = idSegmento.ToString()
            };

            _sincronizacaoSalesforceCO.AddRegistroSincronismo(db, IdentClass.Cliente, idCliente, IdentAction.Update, DateTime.Now,
                System.Text.Json.JsonSerializer.Serialize<ClienteSalesforceBO>(clienteSalesforce));
        }


        public static int AlterarIdSalesforce(BancoDadosBO db, int idCliente, string idFromSalesforce)
        {
            return ClienteDAO.AlterarIdSalesforce(db, idCliente, idFromSalesforce);
        }


        public static void ExcluirCliente(BancoDadosBO db, int idCliente)
        {

            SincronizacaoSalesforceCO _sincronizacaoSalesforceCO = new SincronizacaoSalesforceCO();
            ClienteSalesforceBO clienteSalesforce = new ClienteSalesforceBO()
            {
                IdClass = idCliente,
                Description = cliente.Email,
                Name = cliente.Nome,
                Id = ClienteDAO.GetIdSalesforce(db, idCliente),
                Bairro__c = cliente.Bairro,
                CNAE__c = cliente.Cnae,
                CNPJ__c = cliente.Cnpj,
                Complemento__c = cliente.Complemento,
                DDD__c = cliente.FoneDdd,
                E_mail_c__c = cliente.EmailContato,
                ID_Cliente__c = cliente.IdCliente.ToString(),
                Inscricao_Estadual__c = cliente.Ie,
                Nome_Fantasia__c = cliente.NomeFantasia,
                Numero__c = cliente.NumeroEndereco,
                Phone = cliente.FoneNumero,
                Prius__c = "",
                Segmento__c = cliente.NomeSegmentoOutro
            };

            _sincronizacaoSalesforceCO.AddRegistroSincronismo(db, IdentClass.Cliente, idCliente, IdentAction.Delete, DateTime.Now,
                System.Text.Json.JsonSerializer.Serialize<ClienteSalesforceBO>(clienteSalesforce));
        }


        public static int IncluirCliente(BancoDadosBO db, ClienteBO bo)
        {
            SincronizacaoSalesforceCO _sincronizacaoSalesforceCO = new SincronizacaoSalesforceCO();
            ClienteSalesforceBO clienteSalesforce = new ClienteSalesforceBO()
            {
                IdClass = idCliente,
                Description = bo.NomeHiperador,
                Name = bo.Nome,
                Id = UsuarioDAO.GetIdSalesforce(db, idCliente),
                Atendimento_Hiper__c = bo.IdParceiro.Value.ToString(),
                Bairro__c = bo.Bairro,
                CNAE__c = bo.Cnae,
                CNPJ__c = bo.Cnpj,
                Complemento__c = bo.Complemento,
                DDD__c = bo.FoneDdd,
                E_mail_c__c = bo.EmailContato,
                ID_Cliente__c = bo.IdCliente.ToString(),
                Inscricao_Estadual__c = bo.Ie,
                Nome_Fantasia__c = bo.NomeFantasia,
                Numero__c = bo.NumeroEndereco,
                Phone = bo.FoneNumero,
                Prius__c = "",
                Segmento__c = bo.NomeSegmentoOutro
            };

            _sincronizacaoSalesforceCO.AddRegistroSincronismo(db, IdentClass.Cliente, idCliente, IdentAction.Insert, DateTime.Now,
                System.Text.Json.JsonSerializer.Serialize<ClienteSalesforceBO>(clienteSalesforce));

            return idCliente;
        }


    }
    

}