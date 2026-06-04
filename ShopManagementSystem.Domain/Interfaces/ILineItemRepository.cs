using ShopManagementSystem.Domain.Entities;

namespace ShopManagementSystem.Domain.Interfaces;

public interface ILineItemRepository : IRepository<LineItem>
{
    Task<IReadOnlyList<LineItem>> GetByProductIdAsync(int productId, CancellationToken cancellationToken = default);
}
