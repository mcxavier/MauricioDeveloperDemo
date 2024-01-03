using System;
using System.Collections.Generic;
using Core.QuerysCommands.Commands.Orders.CreateOrder;
using MediatR;

namespace Core.QuerysCommands.Commands.Orders
{
    public class CreadeOrderCommand : IRequest<CreadeOrderResponse>
    {
        public int CatalogId { get; set; }
        public byte Installments { get; set; }
        public string Coupon { get; set; }
        public decimal ShippingPrice { get; set; }
        public CreateOrderRequest_User User { get; set; }
        public CreateOrderRequest_Card Card { get; set; }
        public CreateOrderRequest_Address Address { get; set; }
        public IList<CreateOrderRequest_Product> Items { get; set; }
    }

    public class CreateOrderRequest_User
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
    }


    public class CreateOrderRequest_Card
    {
        public string Holder { get; set; }
        public string Number { get; set; }
        public string SecurityCode { get; set; }
        public string Validation { get; set; }
    }

    public class CreateOrderRequest_Product
    {
        public Guid ProductVariationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal UnitCost { get; set; }
    }

    public class CreateOrderRequest_Address
    {
        public string StreetName { get; set; }
        public string Number { get; set; }
        public string DistrictName { get; set; }
        public string Reference { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Complement { get; set; }
    }
}