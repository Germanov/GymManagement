using GymManagement.Domain.Common.Interfaces;

namespace GymManagement.Domain.Events;

public record SubscriptionDeletedEvent(Guid SubscriptionId) : IDomainEvent;
