using GymManagement.Domain.Gyms;

namespace GymManagement.Application.Common.Interfaces;

public interface IGymsRepository
{
    Task AddGymAsync(Gym gym);

    Task<bool> ExistsAsync(Guid id);

    Task<Gym> GetByIdAsync(Guid id);

    Task<List<Gym>> ListBySubscriptionIdAsync(Guid subscriptionId);

    Task RemoveGymAsync(Gym gym);

    Task RemoveRangeAsync(List<Gym> gymList);

    Task UpdateGymAsync(Gym gym);
}
