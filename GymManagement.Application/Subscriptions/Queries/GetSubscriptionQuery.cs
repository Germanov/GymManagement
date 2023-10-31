using ErrorOr;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Queries;

public record GetSubscriptionQuery(Guid SubscriptionId)
    : IRequest<ErrorOr<Subscription>>;
 