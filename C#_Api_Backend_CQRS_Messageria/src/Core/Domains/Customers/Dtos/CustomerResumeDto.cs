using System;

namespace Core.Dtos
{
    public class CustomerResumeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CPF { get; set; }
        public DateTime? Birth { get; set; }
        public string Phone { get; set; }
        public string Subsidiary { get; set; }
        public string Seller { get; set; }
        public decimal? AverageTicket { get; set; }
        public DateTime? LastBuy { get; set; }
        public bool IsMobilePhone { get; set; }
    }
}