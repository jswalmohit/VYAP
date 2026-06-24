using AutoMapper;
using ShopManagementSystem.Application.DTOs.ProductTypes;
using ShopManagementSystem.Application.Exceptions;
using ShopManagementSystem.Application.Interfaces;
using ShopManagementSystem.Domain.Entities;
using ShopManagementSystem.Domain.Interfaces;

namespace ShopManagementSystem.Application.Services;

public class ProductTypeService : IProductTypeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductTypeService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ProductTypeDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var productTypes = await _unitOfWork.ProductTypes.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<ProductTypeDto>>(productTypes.OrderBy(x=> x.Type));
    }

    public async Task<ProductTypeDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var productType = await _unitOfWork.ProductTypes.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"ProductType with id {id} was not found.");

        return _mapper.Map<ProductTypeDto>(productType);
    }

    public async Task<ProductTypeDto> CreateAsync(CreateProductTypeDto dto, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.ProductTypes.IsTypeUniqueAsync(dto.Type, cancellationToken: cancellationToken))
        {
            throw new BusinessRuleException($"ProductType '{dto.Type}' already exists.");
        }

        var productType = _mapper.Map<ProductType>(dto);
        productType.CreatedDate = DateTime.UtcNow;

        await _unitOfWork.ProductTypes.AddAsync(productType, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProductTypeDto>(productType);
    }

    public async Task<ProductTypeDto> UpdateAsync(int id, UpdateProductTypeDto dto, CancellationToken cancellationToken = default)
    {
        var productType = await _unitOfWork.ProductTypes.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"ProductType with id {id} was not found.");

        if (!await _unitOfWork.ProductTypes.IsTypeUniqueAsync(dto.Type, id, cancellationToken))
        {
            throw new BusinessRuleException($"ProductType '{dto.Type}' already exists.");
        }

        _mapper.Map(dto, productType);
        productType.UpdatedDate = DateTime.UtcNow;

        await _unitOfWork.ProductTypes.UpdateAsync(productType, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProductTypeDto>(productType);
    }
}
