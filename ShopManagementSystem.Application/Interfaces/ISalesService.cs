namespace ShopManagementSystem.Application.Interfaces;

using ShopManagementSystem.Application.DTOs.Sales;

public interface ISalesService
{
    Task<SaleDto> CreateAsync(CreateSaleDto dto, CancellationToken cancellationToken = default);
    Task<SaleDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SaleDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SaleDto>> GetByProductIdAsync(string productId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SaleDto>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SaleDto>> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default);
    Task<SaleDto> UpdateAsync(UpdateSaleDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
