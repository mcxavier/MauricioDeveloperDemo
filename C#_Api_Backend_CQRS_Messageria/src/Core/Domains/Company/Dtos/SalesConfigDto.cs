using System;
using System.Collections.Generic;


namespace Core.Domains.Company.Dtos
{
    public class SalesConfigDto
    {
        public bool? ChangeSeller { get; set; }
        public bool? ChangeOrderStatus { get; set; }
    }
}