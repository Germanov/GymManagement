namespace GymManagement.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; set; }

    protected readonly List<IDomainEvent> domainEvents = [];

    protected Entity(Guid id) => Id = id;

    protected Entity() { }
}
