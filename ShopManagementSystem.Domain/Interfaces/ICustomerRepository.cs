using ShopManagementSystem.Domain.Entities;

namespace ShopManagementSystem.Domain.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByMobileNumberAsync(string mobileNumber, CancellationToken cancellationToken = default);
    Task<bool> IsMobileNumberUniqueAsync(string mobileNumber, int? excludeId = null, CancellationToken cancellationToken = default);
}
