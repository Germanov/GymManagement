namespace GymManagement.Domain.Subscriptions;

public class Subscription
{
    private readonly Guid _adminId;

    public Guid Id { get; }

    public SubscriptionType SubscriptionType { get; }

    public Subscription(
        SubscriptionType subscriptionType,
        Guid adminId,
        Guid? id = null)
    {
        SubscriptionType = subscriptionType;
        _adminId = adminId;
        Id = id ?? Guid.NewGuid();
    }
}
