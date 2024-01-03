using System;
using System.Collections.Generic;
using System.Text;

namespace LinxIO.Queue.Interfaces
{
    public interface ILinxIOLinxUxEnvironmentConfig
    {
        public string AppHost { get; set; }
        public string ServiceHost { get; set; }
        public string Environment { get; set; }
        public string LocationCode { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
