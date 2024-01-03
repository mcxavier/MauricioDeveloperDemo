using System;
using Core.SeedWork;
using Core.SharedKernel;

namespace Core.Domains.Marketing.Models
{
    public class CustomerNotification : Entity<Guid>, IStoreReferenced
    {
        private CustomerNotification() { }

        public CustomerNotification(string stockKeepingUnit, string customerName, string customerEmail)
        {
            this.StockKeepingUnit = stockKeepingUnit;
            this.CustomerName = customerName;
            this.CustomerEmail = customerEmail;
        }

        public string StoreCode { get; set; }

        public string StockKeepingUnit { get; private set; }

        public string CustomerName { get; private set; }

        public string CustomerEmail { get; private set; }

        public bool Notified { get; private set; }

        public DateTime? NotifiedAt { get; private set; }
                
        public void CompleteNotification()
        {
            this.Notified = true;
            this.NotifiedAt = DateTime.Now;
        }
    }
}