using GymManagement.Domain.Gyms;

namespace GymManagement.Application.Common.Interfaces;

public interface IGymRepository
{
    Task AddGymAsync(Gym gym);

    Task<bool> ExistsAsync(Guid id);

    Task<Gym> GetByIdAsync(Guid id);

    Task<List<Gym>> ListBySubscriptionIdAsync(Guid subscriptionId);

    Task RemoveGymAsync(Gym gym);

    Task RemoveGymRangeAsync(List<Gym> gymList);

    Task UpdateGym(Gym gym);
}
