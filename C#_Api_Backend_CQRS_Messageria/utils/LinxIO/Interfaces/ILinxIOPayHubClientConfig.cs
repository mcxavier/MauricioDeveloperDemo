using System;
using System.Collections.Generic;
using System.Text;

namespace LinxIO.Queue.Interfaces
{
    public interface ILinxIOPayHubClientConfig
    {
        public string ApiKey { get; set; }
        public string ClientId { get; set; }
        public bool IsSandBox { get; set; }
    }
}
