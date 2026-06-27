using ShopManagementSystem.Application.DTOs.Customers;

namespace ShopManagementSystem.Application.Interfaces;

public interface ICustomerService
{
    Task<IReadOnlyList<CustomerDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CustomerDto>> SearchAsync(string searchString, CancellationToken cancellationToken = default);
    Task<CustomerDto> GetByIdAsync(string customerId, CancellationToken cancellationToken = default);
    Task<CustomerDto> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<CustomerDto> CreateAsync(CreateCustomerDto dto, CancellationToken cancellationToken = default);
    Task<CustomerDto> UpdateAsync(UpdateCustomerDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(string phoneNumber, CancellationToken cancellationToken = default);
}
