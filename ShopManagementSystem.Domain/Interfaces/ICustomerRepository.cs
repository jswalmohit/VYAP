using ShopManagementSystem.Domain.Entities;

namespace ShopManagementSystem.Domain.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);
    Task<Customer?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Customer>> SearchAsync(string searchString, CancellationToken cancellationToken = default);
    Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber, string? excludeCustomerId = null, CancellationToken cancellationToken = default);
}
