using LinxIO.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinxIO.Dtos
{
    public class LinxIOAllowedTopics
    {
        public string producerApplicationId { get; set; }
        public string topicId { get; set; }
    }
}
