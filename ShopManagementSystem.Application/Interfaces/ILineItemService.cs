using ShopManagementSystem.Application.DTOs.LineItems;

namespace ShopManagementSystem.Application.Interfaces;

public interface ILineItemService
{
    Task<IReadOnlyList<LineItemDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<LineItemDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LineItemDto>> GetByProductIdAsync(string productId, CancellationToken cancellationToken = default);
    Task<LineItemDto> CreateAsync(CreateLineItemDto dto, CancellationToken cancellationToken = default);
    Task<LineItemDto> UpdateAsync(Guid id, UpdateLineItemDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
