using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Core.SharedKernel;
using Newtonsoft.Json;

namespace Infra.QueryCommands.Commands.Topics
{
    public class PublishApplicationProduct : ExtendedEntityWithMetadata<int>, IRequest<Response>
    {
        public IList<ChildEntity>? AdditionalCategories { get; set; }
        public IList<DataIdNameEntity?>? Attributes { get; set; }
        public IdNameEntity Brand { get; set; }
        public ChildEntity? Category { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public IList<IdNameEntity?>? Flags { get; set; }
        public IList<MediaEntity?>? Images { get; set; }
        public IdNameEntity? Manufacturer { get; set; }
        public string ProductId { get; set; }
        public string? PublishedScope { get; set; }
        public IList<ExtendedSkuEntityWithMetadata<string>> Skus { get; set; }
        public IList<IdNameEntity?>? Tags { get; set; }
        public string Title { get; set; }
        public string? Url { get; set; }
        public IList<IdNameEntity?>? Variations { get; set; }
        public IList<MediaEntity?>? Videos { get; set; }
        public string? Warranty { get; set; }
    }
}
