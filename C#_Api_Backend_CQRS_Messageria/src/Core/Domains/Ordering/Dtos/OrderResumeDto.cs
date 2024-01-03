using System;

namespace Core.Domains.Ordering.Dtos
{
    public class OrderResumeDto
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string CEP { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public DateTime? Date { get; set; }
        public decimal? NetValue { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public int Units { get; set; }
        public string Email { get; set; }
        public int OrderType { get; set; }
        public string Phone { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public DateTime? BirthDay { get; set; }
        public int? CardBrand { get; set; }
        public string? SellerName { get; set; }
        public int? SellerId { get; set; }
    }
}