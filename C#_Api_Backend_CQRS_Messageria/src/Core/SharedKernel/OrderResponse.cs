using System.Collections.Generic;

namespace Core.SharedKernel
{
    public class OrderResponse
    {
        public int OrderId { get; set; } = 0;
        public string Reference { get; set; }
        public string AuthenticationUrl { get; set; }
        public string QRCode { get; set; }
        public string QRCodeUrl { get; set; }
        public OrderResponse() { }
        public OrderResponse(int orderId)
        {
            this.OrderId = orderId;
        }
        public OrderResponse(int orderId, string reference)
        {
            this.OrderId = orderId;
            this.Reference = reference;
        }
        public OrderResponse(int orderId, string reference, string authenticationUrl)
        {
            this.OrderId = orderId;
            this.Reference = reference;
            this.AuthenticationUrl = authenticationUrl;
        }
        public OrderResponse(int orderId, string reference, string qrcode, string qrcodeUrl)
        {
            this.OrderId = orderId;
            this.Reference = reference;
            this.QRCode = qrcode;
            this.QRCodeUrl = qrcodeUrl;
        }
    }
}