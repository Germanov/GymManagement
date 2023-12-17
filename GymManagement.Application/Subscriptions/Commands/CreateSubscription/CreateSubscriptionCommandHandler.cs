using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler(ISubscriptionsRepository subscriptionsRepository, IAdminsRepository adminRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateSubscriptionCommand, ErrorOr<Subscription>>
{
    private readonly ISubscriptionsRepository subscriptionsRepository = subscriptionsRepository;
    private readonly IAdminsRepository adminRepository = adminRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<ErrorOr<Subscription>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var admin = await adminRepository.GetByIdAsync(request.AdminId);

        if (admin is null)
        {
            return Error.NotFound(description: "Admin not found.");
        }

        if (admin.SubscriptionId is not null)
        {
            return Error.Conflict(description: "Admin already has an active subscription.");
        }

        var subscription = new Subscription(
            subscriptionType: request.SubscriptionType,
            adminId: request.AdminId);

        admin.SetSubscription(subscription);

        await subscriptionsRepository.AddSubscriptionAsync(subscription);
        await adminRepository.UpdateAsync(admin);
        await unitOfWork.CommitChangesAsync();

        return subscription;
    }
}
