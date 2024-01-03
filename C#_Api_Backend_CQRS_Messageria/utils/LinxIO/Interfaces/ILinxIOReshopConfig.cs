using System;
using System.Collections.Generic;
using System.Text;

namespace LinxIO.Queue.Interfaces
{
    public interface ILinxIOReshopConfig
    {
        public string BaseUrl { get; set; }
        public string Username { get; set; }
        public string Store { get; set; }
        public string Password { get; set; }
    }
}
