using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.DeleteSubscription;

internal class DeleteSubscriptionCommandHandler(ISubscriptionsRepository subscriptionsRepository, IAdminsRepository adminRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteSubscriptionCommand, ErrorOr<Deleted>>
{
    private readonly ISubscriptionsRepository subscriptionsRepository = subscriptionsRepository;
    private readonly IAdminsRepository adminRepository = adminRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<ErrorOr<Deleted>> Handle(DeleteSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionsRepository.GetByIdAsync(command.SubscriptionId);

        if (subscription is null)
        {
            return Error.NotFound(description: "Subscription not found");
        }

        var admin = await adminRepository.GetByIdAsync(subscription.AdminId);

        if (admin is null)
        {
            return Error.Unexpected(description: "Admin not found.");
        }

        admin.DeleteSubscription(command.SubscriptionId);

        await adminRepository.UpdateAsync(admin);
        await unitOfWork.CommitChangesAsync();

        return Result.Deleted;
    }
}