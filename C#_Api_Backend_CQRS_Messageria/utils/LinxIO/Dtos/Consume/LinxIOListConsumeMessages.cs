using LinxIO.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinxIO.Dtos
{
    public class LinxIOListConsumeMessages<BodyType>
    {
        public List<LinxIOConsumeMessages<BodyType>> messages { get; set; }
    }
}
