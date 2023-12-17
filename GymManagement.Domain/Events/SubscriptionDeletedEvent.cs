using GymManagement.Domain.Common;

namespace GymManagement.Domain.Events;

public record SubscriptionDeletedEvent(Guid SubscriptionId) : IDomainEvent;
