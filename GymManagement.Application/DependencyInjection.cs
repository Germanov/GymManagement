using FluentValidation;
using GymManagement.Application.Common.Behaviours;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        service.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));

            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        service.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));

        return service;
    }
}