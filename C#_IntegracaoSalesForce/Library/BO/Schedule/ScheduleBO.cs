using System;

namespace PortalParceiroLibrary.BO.Schedule
{
    public class ScheduleBO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime? UltimaExecucao { get; set; }
    }
}