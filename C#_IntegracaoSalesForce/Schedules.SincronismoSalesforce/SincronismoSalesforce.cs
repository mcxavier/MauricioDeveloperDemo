using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortalParceiroHangfire.SchedulesLogger;
using PortalParceiroLibrary.BO;
using PortalParceiroLibrary.CO.Schedule;
using PortalParceiroLibrary.Services.SincronizacaoSalesforce;

namespace Schedules.SincronismoSalesforce
{
    public class SincronismoSalesforce : ISincronismoSalesforce
    {

        private readonly ISincronizacaoSalesforceService _sincronizacaoSalesforceService;

        public SincronismoSalesforce(ISincronizacaoSalesforceService sincronizacaoSalesforceService)
        {
            _sincronizacaoSalesforceService = sincronizacaoSalesforceService;
        }


        public void Execute(IScheduleLogger logger)
        {
            using (var db = new BancoDadosBO())
            {
                try
                {
                    _sincronizacaoSalesforceService.SincronizaSalesForce(db, logger);
                    new ScheduleCO().AtualizarUltimaExecucao(db, (int)ScheduleCO.Schedules.SincronismoSalesforce, DateTime.Now);
                }
                catch (Exception e)
                {
                    logger?.SetError(e.Message, e.StackTrace);                    
                }

                db.Commit();
            }
        }


    }
}
