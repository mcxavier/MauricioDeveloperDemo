using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Core.SharedKernel;
using Newtonsoft.Json;
using Core.SeedWork;

namespace Infra.QueryCommands.Commands.Topics
{
    public class PublishApplicationSku : ExtendedSkuEntityWithMetadata<int>, IRequest<Response>
    {
        public List<PublishApplicationSku_Attributes> Attributes { get; set; }	
        public bool Enabled { get; set; }
        public PublishApplicationSku_Identifier Identifiers { get; set; }
        public DateTime PublishedAt { get; set; }
        public string PublishedUntil { get; set; }

        public DateTime UpdatedAt { get; set; }
    }


    public class PublishApplicationSku_Identifier
    {
        public string DefaultGtin { get; set; }
        public List<string> Gtins { get; set; }
        public string Isbn { get; set; }
    }

    //public class PublishApplicationSku_Variation
    //{
    //    public List<string> attributes { get; set; }
    //    public PublishApplicationSku_VariationData data { get; set; }
    //    public string id { get; set; }  // COLOR, SIZE, VOLTAGE, FLAVOR, MATERIAL
    //    public string name { get; set; }
    //}

    //public class PublishApplicationSku_VariationData
    //{
    //    public string Id { get; set; }
    //    public string Type { get; set; }  
    //    public string Value { get; set; }
    //}


    public class PublishApplicationSku_Attributes
    {
        public DateTime Data { get; set; }
        public string Format { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }

}
