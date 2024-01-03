using LinxIO.Queue.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinxIO.Queue.Dtos
{
    public class LinxIOReshopConfig : ILinxIOReshopConfig
    {
        public string BaseUrl { get; set; }
        public string Username { get; set; }
        public string Store { get; set; }
        public string Password { get; set; }
    }
}
