using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Queries.GetGym;

internal class GetGymQueryHandler(IGymsRepository gymRepository, ISubscriptionsRepository subscriptionRepository) : IRequestHandler<GetGymQuery, ErrorOr<Gym>>
{
    private readonly IGymsRepository gymRepository = gymRepository;
    private readonly ISubscriptionsRepository subscriptionRepository = subscriptionRepository;

    public async Task<ErrorOr<Gym>> Handle(GetGymQuery query, CancellationToken cancellationToken)
    {
        if (await subscriptionRepository.ExistsAsync(query.SubscriptionId) is false)
        {
            return Error.NotFound(description: "Subscription not found.");
        }

        if (await gymRepository.GetByIdAsync(query.GymId) is not Gym gym)
        {
            return Error.NotFound(description: "Gym not found.");
        }

        return gym;
    }
}
