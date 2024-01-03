
namespace PortalParceiroLibrary.DAO.Contratacao
{
    public class ContratacaoDAO
    {
        public string GetIdSalesforce(BancoDadosBO db, int idContratacao)
        {
            db.AddParam("idContratacao", NpgsqlDbType.Integer, idContratacao);
            StringBuilder query = new StringBuilder();
            query.Append("select id_contratacao from  contratacao WHERE id_contratacao = @idContratacao ");

            return db.ExecuteScalar(query.ToString())?.ToString();
        }

    }
}