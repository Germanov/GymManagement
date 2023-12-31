using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Rooms.Commands.DeleteRoom;

internal class DeleteRoomCommandHandler(IGymsRepository gymsRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteRoomCommand, ErrorOr<Deleted>>
{
    private readonly IGymsRepository gymsRepository = gymsRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<ErrorOr<Deleted>> Handle(DeleteRoomCommand command, CancellationToken cancellationToken)
    {
        var gym = await gymsRepository.GetByIdAsync(command.GymId);

        if (gym is null)
        {
            return Error.NotFound(description: "Gym not found");
        }

        if (gym.HasRoom(command.RoomId) is false)
        {
            return Error.NotFound(description: "Room not found");
        }

        gym.RemoveRoom(command.RoomId);

        await gymsRepository.UpdateGymAsync(gym);
        await unitOfWork.CommitChangesAsync();

        return Result.Deleted;
    }
}