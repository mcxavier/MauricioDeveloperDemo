using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalParceiroLibrary.BO.Salesforce
{
    public class SincronismoSalesforceBO
    {
        public int Id { get; set; }
        public int IdentClass { get; set; }
        public int IdClass { get; set; }
        public int IdentAction { get; set; }
        public DateTime? DataAction { get; set; }
        public string Json { get; set; }
        public DateTime? DataSincronismo { get; set; }
        public string LogMessage { get; set; }
    }
}
