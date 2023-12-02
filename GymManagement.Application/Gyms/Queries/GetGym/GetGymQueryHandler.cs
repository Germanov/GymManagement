using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Queries.GetGym;

internal class GetGymQueryHandler : IRequestHandler<GetGymQuery, ErrorOr<Gym>>
{
    private readonly IGymsRepository gymRepository;
    private readonly ISubscriptionsRepository subscriptionRepository;

    public GetGymQueryHandler(IGymsRepository gymRepository, ISubscriptionsRepository subscriptionRepository)
    {
        this.gymRepository = gymRepository;
        this.subscriptionRepository = subscriptionRepository;
    }

    public async Task<ErrorOr<Gym>> Handle(GetGymQuery query, CancellationToken cancellationToken)
    {
        if (!await subscriptionRepository.ExistsAsync(query.SubscriptionId))
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
