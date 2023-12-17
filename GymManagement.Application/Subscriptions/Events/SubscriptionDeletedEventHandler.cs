using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Events;
using MediatR;

namespace GymManagement.Application.Subscriptions.Events
{
    public class SubscriptionDeletedEventHandler(IUnitOfWork unitOfWork, ISubscriptionsRepository subscriptionsRepository)
        : INotificationHandler<SubscriptionDeletedEvent>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly ISubscriptionsRepository subscriptionsRepository = subscriptionsRepository;

        public async Task Handle(SubscriptionDeletedEvent notification, CancellationToken cancellationToken)
        {
            var subscription = await this.subscriptionsRepository.GetByIdAsync(notification.SubscriptionId);


            if (subscription is null)
            {
                // resilient error handling
                throw new InvalidOperationException();
            }

            await this.subscriptionsRepository.RemoveSubscriptionAsync(subscription);
            await this.unitOfWork.CommitChangesAsync();
        }
    }
}
