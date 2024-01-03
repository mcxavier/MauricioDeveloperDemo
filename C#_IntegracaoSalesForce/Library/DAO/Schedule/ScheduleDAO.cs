using PortalParceiroLibrary.BO.BancoDeDados;
using PortalParceiroLibrary.BO;
using PortalParceiroLibrary.BO.Schedule;
using PortalParceiroLibrary.CO;
using System;
using System.Collections.Generic;

namespace PortalParceiroLibrary.DAO.Schedule
{
    public class ScheduleDAO
    {
        internal int AtualizarUltimaExecucao(BancoDadosBaseBO db, int scheduleId, DateTime ultimaExecucao)
        {
            db.AddParam("id", NpgsqlTypes.NpgsqlDbType.Integer, scheduleId);
            db.AddParam("ultimaExecucao", NpgsqlTypes.NpgsqlDbType.Timestamp, ultimaExecucao);

            return db.ExecuteNonQuery("UPDATE schedules SET ultima_execucao = @ultimaExecucao WHERE id = @id ");
        }

        internal ICollection<ScheduleBO> Listar(BancoDadosBO db)
        {
            return FabricaObjetosCO.InstanciarListaObjetos<ScheduleBO>(db.Select("SELECT * FROM schedules ORDER BY nome "));
        }

        internal ScheduleBO Retornar(BancoDadosBaseBO db, int scheduleId)
        {
            db.AddParam("id", NpgsqlTypes.NpgsqlDbType.Integer, scheduleId);

            var dtSchedules = db.Select("SELECT * FROM schedules WHERE id = @id ");

            return FabricaObjetosCO.InstanciarObjeto<ScheduleBO>(dtSchedules);
        }

        internal bool TodasAsSchedulesRodaramHoje(BancoDadosBO db)
        {
            db.AddParam("hoje", NpgsqlTypes.NpgsqlDbType.Timestamp, DateTime.Now.Date);

            var dtSchedules = db.Select("SELECT id FROM schedules WHERE ultima_execucao < @hoje ");

            return dtSchedules.Rows.Count == 0;
        }
    }
}