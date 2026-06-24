using ShopManagementSystem.Application.DTOs.ProductTypes;

namespace ShopManagementSystem.Application.Interfaces;

public interface IProductTypeService
{
    Task<IReadOnlyList<ProductTypeDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ProductTypeDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ProductTypeDto> CreateAsync(CreateProductTypeDto dto, CancellationToken cancellationToken = default);
    Task<ProductTypeDto> UpdateAsync(int id, UpdateProductTypeDto dto, CancellationToken cancellationToken = default);
}
