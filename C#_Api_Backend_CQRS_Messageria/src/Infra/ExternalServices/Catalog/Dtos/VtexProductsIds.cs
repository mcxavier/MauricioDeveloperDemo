using System.Collections.Generic;

namespace CoreService.IntegrationsViewModels
{

    public class VtexProductsIds
    {

        public Dictionary<string, List<int>> Data { get; set; }

        public Range Range { get; set; }

    }

    public class Range
    {

        public int Total { get; set; }
        public int From  { get; set; }
        public int To    { get; set; }

    }

}