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

    public async Task<Customer?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber, cancellationToken);
    }

    public async Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        return !await DbSet.AnyAsync(
            c => c.PhoneNumber == phoneNumber && (!excludeId.HasValue || c.Id != excludeId.Value),
            cancellationToken);
    }
}
