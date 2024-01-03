using LinxIO.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinxIO.Dtos
{
    public class LinxIOTopics : LinxIOAllowedTopics
    {
        public string queueId { get; set; }
        public int approximateNumberOfMessages { get; set; }
    }
}
