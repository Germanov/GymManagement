using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Queries.ListGym;

internal class ListGymsQueryHandler : IRequestHandler<ListGymsQuery, ErrorOr<List<Gym>>>
{
    private readonly IGymRepository gymRepository;
    private readonly ISubscriptionsRepository subscriptionRepository;

    public ListGymsQueryHandler(IGymRepository gymRepository, ISubscriptionsRepository subscriptionRepository)
    {
        this.gymRepository = gymRepository;
        this.subscriptionRepository = subscriptionRepository;
    }

    public async Task<ErrorOr<List<Gym>>> Handle(ListGymsQuery query, CancellationToken cancellationToken)
    {
        if (!await subscriptionRepository.ExistsAsync(query.SubscriptionId))
        {
            return Error.NotFound(description: "Subscription not found.");
        }

        return await gymRepository.ListBySubscriptionIdAsync(query.SubscriptionId);
    }
}
