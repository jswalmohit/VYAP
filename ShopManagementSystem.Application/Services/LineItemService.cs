using AutoMapper;
using ShopManagementSystem.Application.DTOs.LineItems;
using ShopManagementSystem.Application.Exceptions;
using ShopManagementSystem.Application.Interfaces;
using ShopManagementSystem.Domain.Entities;
using ShopManagementSystem.Domain.Interfaces;

namespace ShopManagementSystem.Application.Services;

public class LineItemService : ILineItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LineItemService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<LineItemDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var lineItems = await _unitOfWork.LineItems.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<LineItemDto>>(lineItems);
    }

    public async Task<LineItemDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var lineItems = await _unitOfWork.LineItems.FindAsync(x => x.Id == id, cancellationToken);
        var lineItem = lineItems.FirstOrDefault();
        
        if (lineItem == null)
        {
            throw new NotFoundException($"LineItem with id {id} was not found.");
        }

        return _mapper.Map<LineItemDto>(lineItem);
    }

    public async Task<IReadOnlyList<LineItemDto>> GetByProductIdAsync(string productId, CancellationToken cancellationToken = default)
    {
        // Verify product exists
        var product = await _unitOfWork.Products.GetByProductIdAsync(productId, cancellationToken);
        if (product == null)
        {
            throw new NotFoundException($"Product with id {productId} was not found.");
        }

        var lineItems = await _unitOfWork.LineItems.GetByProductIdAsync(productId, cancellationToken);
        return _mapper.Map<IReadOnlyList<LineItemDto>>(lineItems);
    }

    public async Task<LineItemDto> CreateAsync(CreateLineItemDto dto, CancellationToken cancellationToken = default)
    {
        // Verify product exists
        var product = await _unitOfWork.Products.GetByProductIdAsync(dto.ProductId, cancellationToken);
        if (product == null)
        {
            throw new NotFoundException($"Product with id {dto.ProductId} was not found.");
        }

        var lineItem = new LineItem
        {
            Id = Guid.NewGuid(),
            ProductId = dto.ProductId,
            PurchasePrice = dto.PurchasePrice,
            Gst = dto.Gst,
            Quantity = dto.Quantity,
            PurchaseDate = dto.PurchaseDate,
            CreatedDate = DateTime.UtcNow,
            SellerGSTIN = dto.SellerGSTIN,
            SellerName = dto.SellerName
        };

        await _unitOfWork.LineItems.AddAsync(lineItem, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LineItemDto>(lineItem);
    }

    public async Task<LineItemDto> UpdateAsync(Guid id, UpdateLineItemDto dto, CancellationToken cancellationToken = default)
    {
        var lineItem = await _unitOfWork.LineItems.FindAsync(x => x.Id == id, cancellationToken);
        var existingItem = lineItem.FirstOrDefault();
        
        if (existingItem == null)
        {
            throw new NotFoundException($"LineItem with id {id} was not found.");
        }

        // Verify product exists if ProductId is being changed
        if (existingItem.ProductId != dto.ProductId)
        {
            var product = await _unitOfWork.Products.GetByProductIdAsync(dto.ProductId, cancellationToken);
            if (product == null)
            {
                throw new NotFoundException($"Product with id {dto.ProductId} was not found.");
            }
        }

        _mapper.Map(dto, existingItem);
        await _unitOfWork.LineItems.UpdateAsync(existingItem, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LineItemDto>(existingItem);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var lineItem = await _unitOfWork.LineItems.FindAsync(x => x.Id == id, cancellationToken);
        var existingItem = lineItem.FirstOrDefault();
        
        if (existingItem == null)
        {
            throw new NotFoundException($"LineItem with id {id} was not found.");
        }

        await _unitOfWork.LineItems.DeleteAsync(existingItem, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
