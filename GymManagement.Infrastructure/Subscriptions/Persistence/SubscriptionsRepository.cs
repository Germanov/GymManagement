using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using GymManagement.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Subscriptions.Persistence;

public class SubscriptionsRepository(GymManagementDbContext dbContext) : ISubscriptionsRepository
{
    private readonly GymManagementDbContext dbContext = dbContext;

    public async Task AddSubscriptionAsync(Subscription subscription)
    {
        await dbContext.Subscriptions.AddAsync(subscription);
    }

    public async Task<bool> ExistsAsync(Guid subscriptionId)
    {
        return await dbContext.Subscriptions
            .AsNoTracking()
            .AnyAsync(subscription => subscription.Id == subscriptionId);
    }

    public async Task<Subscription?> GetByIdAsync(Guid subscriptionId)
    {
        return await dbContext.Subscriptions.FindAsync(subscriptionId);
    }

    public async Task<List<Subscription>> ListAsync()
    {
        return await dbContext.Subscriptions.ToListAsync();
    }

    public Task RemoveSubscriptionAsync(Subscription subscription)
    {
        dbContext.Remove(subscription);

        return Task.CompletedTask;
    }

    public Task UpdateAsync(Subscription subscription)
    {
        dbContext.Update(subscription);

        return Task.CompletedTask;
    }
}