using System;
using System.Collections.Generic;
using Core.Models.Core.Ordering;

namespace Api.Requests
{
    public class CreateOrderRequest
    {

        public int? CatalogId { get; set; }

        public byte Installments { get; set; }

        public string Nsu { get; set; }

        public string Coupon { get; set; }

        public decimal ShippingPrice { get; set; }

        public OrderTypeEnum OrderTypeId { get; set; }

        public decimal QtdeTotal { get; set; }

        public string TransacaoAssociada { get; set; }

        public decimal? ValorBruto { get; set; }

        public decimal? ValorDesconto { get; set; }

        public decimal? ValorDescontosUnico { get; set; }

        public decimal? ValorAcrescimos { get; set; }

        public decimal? ValorLiquido { get; set; }
        public string NumeroOperacao { get; set; }

        public bool? Result { get; set; }

        public string Message { get; set; }

        public bool? IsException { get; set; }

        public bool? Offline { get; set; }

        public CreateOrderRequestUser User { get; set; }

        public CreateOrderRequestCard Card { get; set; }

        public CreateOrderRequestAddress Address { get; set; }

        public IList<CreateOrderRequestProduct> Items { get; set; }

        // TODO: remover isso daqui
        public IList<CreatePaymentRequest_Product> Pagamentos { get; set; }

    }

    public class CreateOrderRequestOrderItemDiscount
    {

        public decimal Value { get; set; }

        public int ReferenceId { get; set; }

        public OrderItemDiscountTypeEnum DiscountType { get; set; }

    }

    // TODO: remover isso daqui 
    public class CreatePaymentRequest_Product
    {
        public int NumeroPagamento { get; set; }

        public string Codigo { get; set; }

        public int Tipo { get; set; }

        public string BinCartao { get; set; }

        public DateTime Vencimento { get; set; }

        public decimal Value { get; set; }

        public bool Result { get; set; }

        public string Message { get; set; }

    }


    public class CreateOrderRequestUser
    {

        public string Name { get; set; }

        public string Cpf { get; set; }

        public string Email { get; set; }

        public string Rg { get; set; }

        public string Phone { get; set; }

        public DateTime? BirthDay { get; set; }

    }


    public class CreateOrderRequestCard
    {

        public string Holder { get; set; }

        public string Number { get; set; }

        public string SecurityCode { get; set; }

        public string Validation { get; set; }

        public byte Installments { get; set; }

    }

    public class CreateOrderRequestProduct
    {

        public int ProductVariationId { get; set; }

        public string Item { get; set; }

        public string Ncm { get; set; }

        public string ReferenceId { get; set; }

        public string SkuCode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal UnitCost { get; set; }

        // TODO: remover isso daqui
        public IList<CreateOrderRequestOrderItemDiscount> OrderDiscounts { get; set; }
    }

    public class CreateOrderRequestAddress
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