using LinxIO.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinxIO.Dtos
{
    public class LinxIOConsumeMessages<BodyType>
    {
        public string receiptId { get; set; }
        public string messageId { get; set; }
        public BodyType body { get; set; }
    }
}
