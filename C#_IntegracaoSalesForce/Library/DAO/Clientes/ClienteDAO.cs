
namespace PortalParceiroLibrary.DAO
{
    public static class ClienteDAO
    {

        public static string GetIdSalesforce(BancoDadosBO db, int idCliente)
        {
            db.AddParam("idCliente", NpgsqlDbType.Integer, idCliente);
            StringBuilder query = new StringBuilder();
            query.Append("select id_salesforce from  cliente WHERE id_cliente = @idCliente ");

            return db.ExecuteScalar(query.ToString())?.ToString();
        }

    }
}