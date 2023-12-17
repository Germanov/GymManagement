using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Events;
using MediatR;

namespace GymManagement.Application.Gyms.Events;

public class SubscriptionDeletedEventHandler(IUnitOfWork unitOfWork, IGymsRepository gymsRepository)
    : INotificationHandler<SubscriptionDeletedEvent>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IGymsRepository gymsRepository = gymsRepository;

    public async Task Handle(SubscriptionDeletedEvent notification, CancellationToken cancellationToken)
    {
        var gyms = await this.gymsRepository.ListBySubscriptionIdAsync(notification.SubscriptionId);

        await this.gymsRepository.RemoveRangeAsync(gyms);
        await this.unitOfWork.CommitChangesAsync();
    }
}
