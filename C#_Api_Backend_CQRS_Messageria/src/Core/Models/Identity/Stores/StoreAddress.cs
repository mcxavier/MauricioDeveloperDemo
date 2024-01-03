using System;
using Core.SeedWork;

namespace Core.Models.Identity.Stores
{
    public class StoreAddress : Entity<Guid>
    {
        public Guid StoreId { get; set; }
        public Store Store { get; set; }
        public string Description { get; set; }
        public string ZipCode { get; set; }
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string Complement { get; set; }
        public string Reference { get; set; }
        public string DistrictName { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
    }
}