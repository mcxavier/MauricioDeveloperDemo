using PortalParceiroLibrary.BO.BancoDeDados;
using PortalParceiroLibrary.BO;
using PortalParceiroLibrary.BO.Schedule;
using PortalParceiroLibrary.CO;
using System;
using System.Collections.Generic;
using PortalParceiroLibrary.BO.Salesforce;

namespace PortalParceiroLibrary.DAO.Salesforce
{
    public class SincronizacaoSalesforceDao
    {
        internal int AddRegistroSincronismo(BancoDadosBaseBO db, int classId, int idRegistro, int idAction, DateTime dataAction, string jsonFile)
        {
            db.AddParam("classId", NpgsqlTypes.NpgsqlDbType.Integer, classId);
            db.AddParam("idRegistro", NpgsqlTypes.NpgsqlDbType.Integer, idRegistro);
            db.AddParam("idAction", NpgsqlTypes.NpgsqlDbType.Integer, idAction);
            db.AddParam("dataAction", NpgsqlTypes.NpgsqlDbType.Timestamp, dataAction);
            db.AddParam("jsonFile", NpgsqlTypes.NpgsqlDbType.Varchar, jsonFile);

            return db.ExecuteNonQuery("INSERT INTO sincronizacao_salesforce " +
                "(ident_class, id_class, ident_action, data_action, json)  " +
                "VALUES " +
                "(@classId, @idRegistro, @idAction, @dataAction, @jsonFile) ");
        }


        internal int DeleteRegistroSincronismo(BancoDadosBaseBO db, int scheduleId)
        {
            db.AddParam("id", NpgsqlTypes.NpgsqlDbType.Integer, scheduleId);

            return db.ExecuteNonQuery("DELETE from sincronizacao_salesforce WHERE id_sincronizacao_salesforce = @id ");
        }


        internal int LogRegistroSincronismo(BancoDadosBaseBO db, int scheduleId, String messageLog)
        {
            db.AddParam("id", NpgsqlTypes.NpgsqlDbType.Integer, scheduleId);
            db.AddParam("message", NpgsqlTypes.NpgsqlDbType.Varchar, messageLog);

            return db.ExecuteNonQuery("UPDATE sincronizacao_salesforce SET data_sincron = current_timestamp, " +
                " message = @message WHERE id_sincronizacao_salesforce = @id ");
        }


        internal ICollection<SincronismoSalesforceBO> Listar(BancoDadosBO db)
        {
            return FabricaObjetosCO.InstanciarListaObjetos<SincronismoSalesforceBO>(db.Select(
                "SELECT id_sincronizacao_salesforce as id,ident_class, id_class, ident_action, data_action, json FROM sincronizacao_salesforce ORDER BY id_sincronizacao_salesforce "));
        }


        internal SincronismoSalesforceBO Retornar(BancoDadosBaseBO db, int scheduleId)
        {
            db.AddParam("id", NpgsqlTypes.NpgsqlDbType.Integer, scheduleId);

            var dtSchedules = db.Select("SELECT * FROM sincronizacao_salesforce WHERE id_sincronizacao_salesforce = @id ");

            return FabricaObjetosCO.InstanciarObjeto<SincronismoSalesforceBO>(dtSchedules);
        }


    }
}