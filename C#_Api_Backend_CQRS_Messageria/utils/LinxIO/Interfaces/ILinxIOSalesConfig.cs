using System;
using System.Collections.Generic;
using System.Text;

namespace LinxIO.Queue.Interfaces
{
    public interface ILinxIOSalesConfig
    {
        public bool ChangeSeller { get; set; }
        public bool ChangeOrderStatus { get; set; }
    }
}
