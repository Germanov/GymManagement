using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.CreateGym;

public class CreateGymCommandHandler(ISubscriptionsRepository subscriptionsRepository, IGymsRepository gymsRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateGymCommand, ErrorOr<Gym>>
{
    private readonly ISubscriptionsRepository subscriptionsRepository = subscriptionsRepository;
    private readonly IGymsRepository gymsRepository = gymsRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<ErrorOr<Gym>> Handle(CreateGymCommand command, CancellationToken cancellationToken)
    {
        var subscription = await this.subscriptionsRepository.GetByIdAsync(command.SubscriptionId);

        if (subscription is null)
        {
            return Error.NotFound(description: "Subscription not found.");
        }

        var gym = new Gym(
            name: command.Name,
            maxRooms: subscription.GetMaxRooms(),
            subscriptionId: subscription.Id);

        var addGymResult = subscription.AddGym(gym);

        if (addGymResult.IsError)
        {
            return addGymResult.Errors;
        }

        await this.subscriptionsRepository.UpdateAsync(subscription);
        await this.gymsRepository.AddGymAsync(gym);
        await this.unitOfWork.CommitChangesAsync();

        return gym;
    }
}
