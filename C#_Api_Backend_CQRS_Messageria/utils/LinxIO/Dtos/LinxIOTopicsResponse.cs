using System;
using System.Collections.Generic;
using System.Text;

namespace LinxIO.Dtos
{
    public class LinxIOTopicsResponse
    {
        public LinxIOTopicsResponse() 
        {
            queues = new List<LinxIOTopics>();
            allowedTopics = new List<LinxIOAllowedTopics>();
        }

        public IList<LinxIOTopics> queues { get; set; }
        public IList<LinxIOAllowedTopics> allowedTopics { get; set; }
    }
}
