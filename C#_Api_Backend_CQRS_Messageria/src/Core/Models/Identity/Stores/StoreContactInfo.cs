using System;
using Core.SeedWork;

namespace Core.Models.Identity.Stores
{
    public class StoreContactInfo : Entity<int>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}