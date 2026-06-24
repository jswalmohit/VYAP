using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Domain.Entities;
using ShopManagementSystem.Domain.Interfaces;
using ShopManagementSystem.Infrastructure.Persistence;

namespace ShopManagementSystem.Infrastructure.Repositories;

public class ProductTypeRepository : Repository<ProductType>, IProductTypeRepository
{
    public ProductTypeRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<bool> IsTypeUniqueAsync(string type, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        return !await DbSet.AnyAsync(
            p => p.Type == type && (!excludeId.HasValue || p.Id != excludeId.Value),
            cancellationToken);
    }
}
