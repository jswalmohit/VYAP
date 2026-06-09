using ShopManagementSystem.Domain.Entities;

namespace ShopManagementSystem.Domain.Interfaces;

public interface ILineItemRepository : IRepository<LineItem>
{
    Task<IReadOnlyList<LineItem>> GetByProductIdAsync(string productId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LineItem>> GetByProductIdForUpdateAsync(string productId, CancellationToken cancellationToken = default);
}
