using System.Collections.Generic;
using MediatR;

namespace Core.SeedWork
{
    public interface IEntity
    {
        object Id { get; set; }
        void AddDomainEvent(INotification eventItem);
        void RemoveDomainEvent(INotification eventItem);
        void ClearDomainEvents();
    }

    public interface IEntity<T> : IEntity
    {
        new T Id { get; set; }
    }

    public abstract class Entity<T> : IEntity<T>
    {
        public T Id { get; set; }

        object IEntity.Id
        {
            get => this.Id;
            set => this.Id = (T)value;
        }

        private List<INotification> _domainEvents;

        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            this._domainEvents ??= new List<INotification>();
            this._domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            this._domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            this._domainEvents?.Clear();
        }
    }
}