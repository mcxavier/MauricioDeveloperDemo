using Core.Models.Identity.Companies;
using Core.Repositories;
using LinxIO.Interfaces;
using Microsoft.Azure.WebJobs;

namespace LinxIO.Queue
{
    public class FunctionsScheduled
    {
        private readonly IQueueService queueService;
        private readonly ICompanyRepository companyRepository;

        public FunctionsScheduled(IQueueService queueService, ICompanyRepository companyRepository)
        {
            this.queueService = queueService;
            this.companyRepository = companyRepository;
        }

#if !DEBUG
        [Singleton]
        public void DoIntegrationLinxIO([TimerTrigger("*/15 * * * *", RunOnStartup = true)] TimerInfo timerInfo)
        {
            try
            {
                var companies = companyRepository.ListCompanyIntegrateIOAsync(CompanySettingsType.LinxIOConfig).Result;
                foreach (var company in companies)
                {
                    queueService.SendMessage("linxio-queue", company.CompanyId.ToString());
                }
            }
            catch (System.Exception ex)
            {
                //TODO: Criar log
            }
        }
#endif
    }
}
