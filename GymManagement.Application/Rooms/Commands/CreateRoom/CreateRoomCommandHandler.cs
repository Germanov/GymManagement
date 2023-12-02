using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Rooms;
using MediatR;

namespace GymManagement.Application.Rooms.Commands.CreateRoom;

public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, ErrorOr<Room>>
{
    private readonly ISubscriptionsRepository subscriptionsRepository;
    private readonly IGymsRepository gymsRepository;
    private readonly IUnitOfWork unitOfWork;

    public CreateRoomCommandHandler(ISubscriptionsRepository subscriptionsRepository, IGymsRepository gymsRepository, IUnitOfWork unitOfWork)
    {
        this.subscriptionsRepository = subscriptionsRepository;
        this.gymsRepository = gymsRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Room>> Handle(CreateRoomCommand command, CancellationToken cancellationToken)
    {
        var gym = await gymsRepository.GetByIdAsync(command.GymId);

        if (gym is null)
        {
            return Error.NotFound(description: "Gym not found.");
        }

        var subscription = await subscriptionsRepository.GetByIdAsync(gym.SubscriptionId);

        if (subscription is null)
        {
            return Error.Unexpected(description: "Subscription not found.");
        }

        var room = new Room(
            name: command.RoomName,
            gymId: gym.Id,
            maxDailySessions: subscription.GetMaxDailySessions());

        var addGymResult = gym.AddRoom(room);

        if (addGymResult.IsError)
        {
            return addGymResult.Errors;
        }

        await gymsRepository.UpdateGymAsync(gym);
        await unitOfWork.CommitChangesAsync();

        return room;
    }
}

