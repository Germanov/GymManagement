using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.CreateGym;

public class CreateGymCommandHandler : IRequestHandler<CreateGymCommand, ErrorOr<Gym>>
{
    private readonly ISubscriptionsRepository subscriptionsRepository;
    private readonly IGymRepository gymsRepository;
    private readonly IUnitOfWork unitOfWork;

    public CreateGymCommandHandler(ISubscriptionsRepository subscriptionsRepository, IGymRepository gymRepository, IUnitOfWork unitOfWork)
    {
        this.subscriptionsRepository = subscriptionsRepository;
        this.gymsRepository = gymRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Gym>> Handle(CreateGymCommand command, CancellationToken cancellationToken)
    {
        var subscription = await this.subscriptionsRepository.GetByIdAsync(command.SubscriptionId);

        if (subscription == null)
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

        await subscriptionsRepository.UpdateAsync(subscription);
        await gymsRepository.AddGymAsync(gym);
        await unitOfWork.CommitChangesAsync();

        return gym;
    }
}
