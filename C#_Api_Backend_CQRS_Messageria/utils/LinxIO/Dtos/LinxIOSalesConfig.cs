using LinxIO.Queue.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinxIO.Queue.Dtos
{
    public class LinxIOSalesConfig : ILinxIOSalesConfig
    {
        public bool ChangeSeller { get; set; }
        public bool ChangeOrderStatus { get; set; }
    }
}
