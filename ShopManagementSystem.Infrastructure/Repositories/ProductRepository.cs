using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Domain.Entities;
using ShopManagementSystem.Domain.Interfaces;
using ShopManagementSystem.Infrastructure.Persistence;

namespace ShopManagementSystem.Infrastructure.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Product?> GetByProductIdAsync(string productId, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(p => p.ProductId == productId, cancellationToken);
    }

    public async Task<IReadOnlyList<Product>> SearchAsync(string term, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            return await GetAllAsync(cancellationToken);
        }

        term = term.Trim().ToLower();

        return await DbSet
            .AsNoTracking()
            .Where(p => p.ProductName.ToLower().Contains(term) || p.ProductId.ToLower().Contains(term))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsProductIdUniqueAsync(string productId, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        return !await DbSet.AnyAsync(
            p => p.ProductId == productId && (!excludeId.HasValue || p.Id != excludeId.Value),
            cancellationToken);
    }
}
