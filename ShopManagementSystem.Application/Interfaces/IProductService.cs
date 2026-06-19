using ShopManagementSystem.Application.DTOs.Products;

namespace ShopManagementSystem.Application.Interfaces;

public interface IProductService
{
    Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ProductDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProductDto>> SearchAsync(string term, CancellationToken cancellationToken = default);
    Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default);
    Task<ProductDto> UpdateAsync(int id, UpdateProductDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ProductDto> DecreaseQuantityAsync(string productId, int quantity, CancellationToken cancellationToken = default);
}
