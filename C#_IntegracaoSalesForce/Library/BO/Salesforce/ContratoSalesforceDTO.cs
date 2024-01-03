using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalParceiroLibrary.BO.Salesforce
{
    public class ContratoSalesforceDto: SalesforceDto
    {
        public string Description { get; set; }
        public string accountId { get; set; }
        public decimal? Valor_Assinatura__c { get; set; }
        public string Plano__c { get; set; }
        public string Produto__c { get; set; }
    }
}
