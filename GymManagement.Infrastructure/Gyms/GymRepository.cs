﻿using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Gyms;
using GymManagement.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Gyms;

public class GymRepository : IGymsRepository
{
    private readonly GymManagementDbContext dbContext;

    public GymRepository(GymManagementDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddGymAsync(Gym gym)
    {
        await dbContext.Gyms.AddAsync(gym);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await dbContext.Gyms.AsNoTracking().AnyAsync(gym => gym.Id == id);
    }

    public async Task<Gym?> GetByIdAsync(Guid id)
    {
        return await dbContext.Gyms.FirstOrDefaultAsync(gym => gym.Id == id);
    }

    public async Task<List<Gym>> ListBySubscriptionIdAsync(Guid subscriptionId)
    {
        return await dbContext.Gyms
            .Where(gym => gym.SubscriptionId == subscriptionId)
            .ToListAsync();
    }

    public Task RemoveGymAsync(Gym gym)
    {
        dbContext.Remove(gym);

        return Task.CompletedTask;
    }

    public Task RemoveGymRangeAsync(List<Gym> gymList)
    {
        dbContext.RemoveRange(gymList);

        return Task.CompletedTask;
    }

    public Task UpdateGymAsync(Gym gym)
    {
        dbContext.Update(gym);

        return Task.CompletedTask;
    }
}
