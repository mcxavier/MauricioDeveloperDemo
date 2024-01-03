using LinxIO.Queue.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinxIO.Queue.Dtos
{
    public class LinxIOPayHubClientConfig : ILinxIOPayHubClientConfig
    {
        public string ApiKey { get; set; }
        public string ClientId { get; set; }
        public bool IsSandBox { get; set; }
    }
}
