using ShopManagementSystem.Domain.Entities;

namespace ShopManagementSystem.Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetByProductIdAsync(string productId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> SearchAsync(string term, CancellationToken cancellationToken = default);
    Task<bool> IsProductIdUniqueAsync(string productId, int? excludeId = null, CancellationToken cancellationToken = default);
}
