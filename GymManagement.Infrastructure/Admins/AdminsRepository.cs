using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins;
using GymManagement.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Admins;

public class AdminsRepository(GymManagementDbContext dbContext) : IAdminsRepository
{
    private readonly GymManagementDbContext dbContext = dbContext;

    public Task<Admin?> GetByIdAsync(Guid adminId)
    {
        var admin = dbContext.Admins.FirstOrDefaultAsync(a => a.Id == adminId);

        return admin;
    }

    public Task UpdateAsync(Admin admin)
    {
        dbContext.Admins.Update(admin);

        return Task.CompletedTask;
    }
}