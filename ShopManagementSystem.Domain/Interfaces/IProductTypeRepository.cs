using ShopManagementSystem.Domain.Entities;

namespace ShopManagementSystem.Domain.Interfaces;

public interface IProductTypeRepository : IRepository<ProductType>
{
    Task<bool> IsTypeUniqueAsync(string type, int? excludeId = null, CancellationToken cancellationToken = default);
}
