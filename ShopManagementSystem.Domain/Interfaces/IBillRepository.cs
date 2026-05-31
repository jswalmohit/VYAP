using ShopManagementSystem.Domain.Entities;

namespace ShopManagementSystem.Domain.Interfaces;

public interface IBillRepository : IRepository<Bill>
{
    Task<IReadOnlyList<Bill>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);
    Task<Bill?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<string> GenerateBillNumberAsync(CancellationToken cancellationToken = default);
}
