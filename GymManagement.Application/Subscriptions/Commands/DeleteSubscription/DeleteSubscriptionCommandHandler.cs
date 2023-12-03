using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.DeleteSubscription;

internal class DeleteSubscriptionCommandHandler : IRequestHandler<DeleteSubscriptionCommand, ErrorOr<Deleted>>
{
    private readonly ISubscriptionsRepository subscriptionsRepository;
    private readonly IAdminsRepository adminRepository;
    private readonly IGymsRepository gymsRepository;
    private readonly IUnitOfWork unitOfWork;

    public DeleteSubscriptionCommandHandler(ISubscriptionsRepository subscriptionsRepository, IAdminsRepository adminRepository, IGymsRepository gymsRepository, IUnitOfWork unitOfWork)
    {
        this.subscriptionsRepository = subscriptionsRepository;
        this.adminRepository = adminRepository;
        this.gymsRepository = gymsRepository;
        this.unitOfWork = unitOfWork;
    }

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

        var gymsToDelete = await gymsRepository.ListBySubscriptionIdAsync(command.SubscriptionId);

        await adminRepository.UpdateAsync(admin);
        await subscriptionsRepository.RemoveSubscriptionAsync(subscription);
        await gymsRepository.RemoveGymRangeAsync(gymsToDelete);
        await unitOfWork.CommitChangesAsync();

        return Result.Deleted;
    }
}