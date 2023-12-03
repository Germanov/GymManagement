using GymManagement.Application.Common.Interfaces;
using GymManagement.Infrastructure.Admins;
using GymManagement.Infrastructure.Common.Persistence;
using GymManagement.Infrastructure.Gyms;
using GymManagement.Infrastructure.Subscriptions.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service)
    {
        service.AddDbContext<GymManagementDbContext>(options =>
        {
            options.UseSqlite("Data Source = GymManagement.db");
        });

        service.AddScoped<IAdminsRepository, AdminsRepository>();
        service.AddScoped<IGymsRepository, GymsRepository>();
        service.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>();

        service.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<GymManagementDbContext>());

        return service;
    }
}