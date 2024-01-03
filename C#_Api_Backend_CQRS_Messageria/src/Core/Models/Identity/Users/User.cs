using System;
using Core.Models.Identity.Companies;
using Core.SeedWork;

namespace Core.Models.Identity.Users
{
    public class User : Entity<Guid>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
    }
}