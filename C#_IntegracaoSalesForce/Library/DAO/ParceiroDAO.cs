namespace PortalParceiroLibrary.DAO
{
    public static class ParceiroDAO
    {

        public static string GetIdSalesforce(BancoDadosBO db, int idParceiro)
        {
            db.AddParam("idParceiro", NpgsqlDbType.Integer, idParceiro);
            StringBuilder query = new StringBuilder();
            query.Append("select id_salesforce from  parceiro WHERE id_parceiro = @idParceiro ");

            return db.ExecuteScalar(query.ToString())?.ToString();
        }

    }
}