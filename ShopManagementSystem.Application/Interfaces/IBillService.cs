using ShopManagementSystem.Application.DTOs.Bills;

namespace ShopManagementSystem.Application.Interfaces;

public interface IBillService
{
    Task<BillDto> CreateAsync(CreateBillDto dto, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BillDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BillDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BillDto>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);
}
