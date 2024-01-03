
namespace PortalParceiroLibrary.DAO
{
    public static class UsuarioDAO
    {
        public static int AlterarIdSalesforce(BancoDadosBO db, int idUsuario, string idFromSalesforce)
        {
            db.AddParam("idUsuario", NpgsqlDbType.Integer, idUsuario);
            db.AddParam("idSalesforce", NpgsqlDbType.Varchar, idFromSalesforce);

            StringBuilder query = new StringBuilder();
            query.Append("UPDATE usuario SET id_salesforce = @idSalesforce ");
            query.Append("WHERE id_usuario = @idUsuario ");

            return db.ExecuteNonQuery(query.ToString());
        }


        public static string GetIdSalesforce(BancoDadosBO db, int idUsuario)
        {
            db.AddParam("idUsuario", NpgsqlDbType.Integer, idUsuario);
            StringBuilder query = new StringBuilder();
            query.Append("select id_salesforce from  usuario WHERE id_usuario = @idUsuario ");

            return db.ExecuteScalar(query.ToString())?.ToString();
        }

    }
}