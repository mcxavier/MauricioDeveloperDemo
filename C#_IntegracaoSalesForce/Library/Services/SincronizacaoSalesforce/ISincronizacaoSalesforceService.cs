using PortalParceiroHangfire.SchedulesLogger;
using PortalParceiroLibrary.BO;

namespace PortalParceiroLibrary.Services.SincronizacaoSalesforce
{
    public interface ISincronizacaoSalesforceService
    {
        void SincronizaSalesForce(BancoDadosBO db, IScheduleLogger logger);
    }
}
