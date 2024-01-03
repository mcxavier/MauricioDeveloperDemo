using System;
using System.Collections.Generic;

namespace CoreService.IntegrationsViewModels
{

    public class VtexProductModel
    {

        public int    Id          { get; set; }
        public string Name        { get; set; }
        public string Description { get; set; }
        public bool   IsActive    { get; set; }
        public string Category    { get; set; }

    }

}