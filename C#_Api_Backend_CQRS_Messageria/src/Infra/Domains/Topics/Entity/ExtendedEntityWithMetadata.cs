using System;
using Core.SeedWork;

namespace Infra.QueryCommands.Commands.Topics
{
    public abstract class ExtendedEntityWithMetadata<T> : EntityWithMetadata<T>
    {
        public DateTime PublishedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
