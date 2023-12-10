using ErrorOr;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.CreateGym;

public class CreateGymCommandBehavior : IPipelineBehavior<CreateGymCommand, ErrorOr<Gym>>
{
    public async Task<ErrorOr<Gym>> Handle(CreateGymCommand request, RequestHandlerDelegate<ErrorOr<Gym>> next, CancellationToken cancellationToken)
    {
        var validator = new CreateGymCommandValidator();

        var validatorResult = await validator.ValidateAsync(request);

        if (!validatorResult.IsValid)
        {
            return validatorResult.Errors
                .Select(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage))
                .ToArray();
        }

        return await next();
    }
}
