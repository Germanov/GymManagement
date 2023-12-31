using Ardalis.SmartEnum;

namespace GymManagement.Domain.Subscriptions;

public class SubscriptionType(string name, int value) : SmartEnum<SubscriptionType>(name, value)
{
    public static readonly SubscriptionType Free = new(nameof(Free), 0);
    public static readonly SubscriptionType Starter = new(nameof(Starter), 1);
    public static readonly SubscriptionType Pro = new(nameof(Pro), 2);
}