using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Domain.Entities;
using ShopManagementSystem.Domain.Interfaces;
using ShopManagementSystem.Infrastructure.Persistence;

namespace ShopManagementSystem.Infrastructure.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Customer?> GetByMobileNumberAsync(string mobileNumber, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(c => c.MobileNumber == mobileNumber, cancellationToken);
    }

    public async Task<bool> IsMobileNumberUniqueAsync(string mobileNumber, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        return !await DbSet.AnyAsync(
            c => c.MobileNumber == mobileNumber && (!excludeId.HasValue || c.Id != excludeId.Value),
            cancellationToken);
    }
}
