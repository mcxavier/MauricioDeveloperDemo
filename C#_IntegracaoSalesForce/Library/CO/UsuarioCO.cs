using PortalParceiroLibrary.BO.Salesforce;
using PortalParceiroLibrary.CO.Salesforce;
using static PortalParceiroLibrary.CO.Salesforce.SincronizacaoSalesforceCO;

namespace PortalParceiroLibrary.CO
{
    public static class UsuarioCO
    {

        public static void AlterarUsuario(BancoDadosBO db, int idUsuario, int idParceiro, string email, string nome, bool ativo, bool vendedor, short idPerfilUsuario, bool atendente, int? codigoAtendimento = null)
        {
            UsuarioBO usuario = new UsuarioBO();
            usuario.IdUsuario = idUsuario;
            usuario.IdParceiro = idParceiro;
            usuario.Nome = nome;
            usuario.Email = email;
            usuario.Ativo = ativo;
            usuario.IdPerfilUsuario = (PerfilUsuario)Enum.Parse(typeof(PerfilUsuario), idPerfilUsuario.ToString());
            usuario.Vendedor = vendedor;
            usuario.Atendente = atendente;
            usuario.CodigoAtendimento = codigoAtendimento;

            UsuarioDAO.AlterarUsuario(db, usuario);

            SincronizacaoSalesforceCO _sincronizacaoSalesforceCO = new SincronizacaoSalesforceCO();
            UsuarioSalesforceBO usuarioSalesforce = new UsuarioSalesforceBO() { 
               IdClass = idUsuario, 
               Description = nome,
               Name = nome,
               Id = UsuarioDAO.GetIdSalesforce(db, idUsuario),
                Bairro__c = "",
                CNAE__c = "",
                CNPJ__c = "",
                Complemento__c = "",
                DDD__c = "",
                E_mail_c__c = email,
                ID_Cliente__c = "",
                Inscricao_Estadual__c = "",
                Nome_Fantasia__c = "",
                Numero__c = "",
                Phone = "",
                Prius__c = "",
                Segmento__c = ""
            };

            _sincronizacaoSalesforceCO.AddRegistroSincronismo(db, IdentClass.Usuario, idUsuario, IdentAction.Update, DateTime.Now,
                JsonSerializer.Serialize<UsuarioSalesforceBO>(usuarioSalesforce));
		}
 
        public static int AlterarIdSalesforce(BancoDadosBO db, int idUsuario, string idFromSalesforce)
        {
            return UsuarioDAO.AlterarIdSalesforce(db, idUsuario, idFromSalesforce);
        }


        public static void ExcluirUsuario(BancoDadosBO db, int idUsuario)
        {
            SincronizacaoSalesforceCO _sincronizacaoSalesforceCO = new SincronizacaoSalesforceCO();
            UsuarioSalesforceBO usuarioSalesforce = new UsuarioSalesforceBO()
            {
                IdClass = idUsuario,
                Id = UsuarioDAO.GetIdSalesforce(db, idUsuario)
            };

            _sincronizacaoSalesforceCO.AddRegistroSincronismo(db, IdentClass.Usuario, idUsuario, IdentAction.Delete, DateTime.Now,
                JsonSerializer.Serialize<UsuarioSalesforceBO>(usuarioSalesforce));
        }

        public static int IncluirUsuario(BancoDadosBO db, string email, string nome, bool ativo, bool vendedor, int idParceiro, short idPerfilUsuario, int idUsuarioCadastro, bool atendente, int? codigoAtendimento = null)
        {
            UsuarioBO usuario = new UsuarioBO
            {
                Atendente = atendente,
                Ativo = ativo,
                Email = email,
                IdParceiro = idParceiro,
                IdPerfilUsuario = (PerfilUsuario)Enum.Parse(typeof(PerfilUsuario), idPerfilUsuario.ToString()),
                Nome = nome,
                Senha = Criptografia.GeraHashMD5(senha),
                Vendedor = vendedor,
            };

            usuario.HashAtivacao = Criptografia.GeraHashMD5(usuario.Senha);
            usuario.IdParceiro = idParceiro;
            usuario.IdPerfilUsuario = (PerfilUsuario)Enum.Parse(typeof(PerfilUsuario), idPerfilUsuario.ToString());
            usuario.Atendente = atendente;
            usuario.CodigoAtendimento = codigoAtendimento;
            int idUsuario = UsuarioDAO.IncluirUsuario(db, usuario);

            SincronizacaoSalesforceCO _sincronizacaoSalesforceCO = new SincronizacaoSalesforceCO();
            UsuarioSalesforceBO usuarioSalesforce = new UsuarioSalesforceBO()
            {
                IdClass = idUsuario,
                Description = nome,
                Name = nome,
                Id = UsuarioDAO.GetIdSalesforce(db, idUsuario),
                Bairro__c = "",
                CNAE__c = "",
                CNPJ__c = "",
                Complemento__c = "",
                DDD__c = "",
                E_mail_c__c = usuario.Email,
                ID_Cliente__c = "",
                Inscricao_Estadual__c = "",
                Nome_Fantasia__c = "",
                Numero__c = "",
                Phone = "",
                Prius__c = "",
                Segmento__c = ""
            };

            _sincronizacaoSalesforceCO.AddRegistroSincronismo(db, IdentClass.Usuario, idUsuario, IdentAction.Insert, DateTime.Now,
                JsonSerializer.Serialize<UsuarioSalesforceBO>(usuarioSalesforce));

            return idUsuario;
        }

    }
}