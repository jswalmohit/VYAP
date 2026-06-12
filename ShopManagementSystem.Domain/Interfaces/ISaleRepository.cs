namespace ShopManagementSystem.Domain.Interfaces;

using ShopManagementSystem.Domain.Entities;

public interface ISaleRepository
{
    Task<Sale?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Sale>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Sale>> GetByProductIdAsync(string productId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Sale>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Sale>> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default);
    Task AddAsync(Sale sale, CancellationToken cancellationToken = default);
    Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
