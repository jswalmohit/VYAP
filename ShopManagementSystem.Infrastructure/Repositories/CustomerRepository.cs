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

    public async Task<Customer?> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);
    }

    public async Task<Customer?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber, cancellationToken);
    }

    public async Task<IReadOnlyList<Customer>> SearchAsync(string searchString, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchString))
        {
            return Array.Empty<Customer>();
        }

        var normalizedSearch = $"%{searchString.Trim().ToLower()}%";

        return await DbSet.AsNoTracking()
            .Where(c => EF.Functions.Like(c.CustomerId.ToLower(), normalizedSearch)
                     || EF.Functions.Like(c.CustomerName.ToLower(), normalizedSearch)
                     || EF.Functions.Like(c.PhoneNumber.ToLower(), normalizedSearch))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber, string? excludeCustomerId = null, CancellationToken cancellationToken = default)
    {
        return !await DbSet.AnyAsync(
            c => c.PhoneNumber == phoneNumber && (string.IsNullOrEmpty(excludeCustomerId) || c.CustomerId != excludeCustomerId),
            cancellationToken);
    }
}
