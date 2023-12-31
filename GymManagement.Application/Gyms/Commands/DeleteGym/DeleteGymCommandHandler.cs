using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.DeleteGym;

public class DeleteGymCommandHandler(ISubscriptionsRepository subscriptionsRepository, IGymsRepository gymRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteGymCommand, ErrorOr<Deleted>>
{
    private readonly ISubscriptionsRepository subscriptionsRepository = subscriptionsRepository;
    private readonly IGymsRepository gymRepository = gymRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<ErrorOr<Deleted>> Handle(DeleteGymCommand command, CancellationToken cancellationToken)
    {
        var gym = await gymRepository.GetByIdAsync(command.GymId);

        if (gym == null)
        {
            return Error.NotFound(description: "Gym not found.");
        }

        var subscription = await subscriptionsRepository.GetByIdAsync(command.SubscriptionId);

        if (subscription == null)
        {
            return Error.NotFound(description: "Subscription not found.");
        }

        if (!subscription.HasGym(command.GymId))
        {
            return Error.Unexpected(description: "Gym not found.");
        }

        subscription.RemoveGym(command.SubscriptionId);

        await subscriptionsRepository.UpdateAsync(subscription);
        await gymRepository.RemoveGymAsync(gym);
        await unitOfWork.CommitChangesAsync();

        return Result.Deleted;
    }
}
