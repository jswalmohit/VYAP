using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Domain.Entities;
using ShopManagementSystem.Domain.Interfaces;
using ShopManagementSystem.Infrastructure.Persistence;

namespace ShopManagementSystem.Infrastructure.Repositories;

public class LineItemRepository : Repository<LineItem>, ILineItemRepository
{
    public LineItemRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<LineItem>> GetByProductIdAsync(string productId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(li => li.ProductId == productId)
            .ToListAsync(cancellationToken);
    }
}
