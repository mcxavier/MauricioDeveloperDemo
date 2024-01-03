using System;

namespace Core.SeedWork
{
    public interface IEntityWithMetadata
    {
        DateTime? CreatedAt { get; set; }
        DateTime? ModifiedAt { get; set; }
        string CreatedBy { get; set; }
        string ModifiedBy { get; set; }
        bool IsActive { get; set; }
    }

    public abstract class EntityWithMetadata<T> : Entity<T>, IEntityWithMetadata
    {
        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? ModifiedAt { get; set; } = null;
        public string CreatedBy { get; set; } = null;
        public string ModifiedBy { get; set; } = null;
        public bool IsActive { get; set; } = true;
    }
}