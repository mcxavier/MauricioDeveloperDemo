using System.Collections.Generic;

namespace Infra.QueryCommands.Commands.Topics
{
    public class MediaEntity : ExtendedEntityWithMetadata<string>
    {
        public int Height { get; set; }
        public IList<string> SkuIds { get; set; }
        public string Url { get; set; }
        public int Width { get; set; }
    }
}
