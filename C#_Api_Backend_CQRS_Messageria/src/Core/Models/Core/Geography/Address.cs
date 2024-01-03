using Core.SeedWork;

namespace Core.Models.Core.Geography
{
    public class Address : Entity<int>
    {
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