using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Queries;

public class GetSubscriptionQueryHandler(ISubscriptionsRepository subscriptionsRepository) : IRequestHandler<GetSubscriptionQuery, ErrorOr<Subscription>>
{
    private readonly ISubscriptionsRepository subscriptionsRepository = subscriptionsRepository;

    public async Task<ErrorOr<Subscription>> Handle(GetSubscriptionQuery request, CancellationToken cancellationToken)
    {
        var subcription = await subscriptionsRepository.GetByIdAsync(request.SubscriptionId);

        return subcription is null
            ? Error.NotFound(description: "Subscription not found.")
            : subcription;
    }
}
