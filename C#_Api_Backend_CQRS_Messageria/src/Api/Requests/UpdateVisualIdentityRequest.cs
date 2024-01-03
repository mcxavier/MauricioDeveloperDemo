using System;
using System.Collections.Generic;

namespace Api.Requests
{
    public class UpdateVisualIdentityRequest
    {
        public string StorePortal { get; set; }
        public string? ZipCode { get; set; }
        public int? ProductsQuant { get; set; }
        public decimal? Freight { get; set; }
        public decimal? Additional { get; set; }
        public string MainColor { get; set; }
        public string SecondaryColor { get; set; }
        public int? Range { get; set; }
        public string? WhatsApp { get; set; }
        public string? FooterMessage { get; set; }
    }
}