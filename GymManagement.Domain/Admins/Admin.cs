﻿using GymManagement.Domain.Common;
using GymManagement.Domain.Events;
using GymManagement.Domain.Subscriptions;
using Throw;

namespace GymManagement.Domain.Admins;

public class Admin : Entity
{
    public Guid UserId { get; }
    public Guid? SubscriptionId { get; private set; } = null;

    public Admin(Guid userId, Guid? subscriptionId = null, Guid? id = null)
        : base(id ?? Guid.NewGuid())
    {
        UserId = userId;
        SubscriptionId = subscriptionId;
    }

    public void SetSubscription(Subscription subscription)
    {
        SubscriptionId.HasValue.Throw().IfTrue();

        SubscriptionId = subscription.Id;
    }

    public void DeleteSubscription(Guid subscriptionId)
    {
        SubscriptionId.ThrowIfNull().IfNotEquals(subscriptionId);

        SubscriptionId = null;

        domainEvents.Add(new SubscriptionDeletedEvent(subscriptionId));
    }

    private Admin() { }
}