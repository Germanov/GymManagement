﻿using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Queries.ListGym;

internal class ListGymsQueryHandler(IGymsRepository gymRepository, ISubscriptionsRepository subscriptionRepository) : IRequestHandler<ListGymsQuery, ErrorOr<List<Gym>>>
{
    private readonly IGymsRepository gymRepository = gymRepository;
    private readonly ISubscriptionsRepository subscriptionRepository = subscriptionRepository;

    public async Task<ErrorOr<List<Gym>>> Handle(ListGymsQuery query, CancellationToken cancellationToken)
    {
        if (await subscriptionRepository.ExistsAsync(query.SubscriptionId) is false)
        {
            return Error.NotFound(description: "Subscription not found.");
        }

        return await gymRepository.ListBySubscriptionIdAsync(query.SubscriptionId);
    }
}
