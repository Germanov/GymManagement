using GymManagement.Domain.Common.Interfaces;

namespace GymManagement.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; set; }

    protected readonly List<IDomainEvent> domainEvents = [];

    protected Entity(Guid id) => Id = id;

    public List<IDomainEvent> PopDomainEvents()
    {
        var copy = this.domainEvents.ToList();

        domainEvents.Clear();

        return copy;
    }

    protected Entity() { }
}
