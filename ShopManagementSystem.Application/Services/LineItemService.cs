using AutoMapper;
using ShopManagementSystem.Application.DTOs.LineItems;
using ShopManagementSystem.Application.Exceptions;
using ShopManagementSystem.Application.Interfaces;
using ShopManagementSystem.Domain.Entities;
using ShopManagementSystem.Domain.Interfaces;
using System.Data;

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

    private static int CalculateQuantityDelta(int originalQuantity, int newQuantity)
    {
        return newQuantity - originalQuantity;
    }

    private async Task<Product> GetProductAsync(string productId, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByProductIdAsync(productId, cancellationToken);
        if (product == null)
        {
            throw new NotFoundException($"Product with id {productId} was not found.");
        }

        return product;
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
        var lineItem = new LineItem
        {
            Id = Guid.NewGuid(),
            ProductId = dto.ProductId,
            PurchasePrice = dto.PurchasePrice,
            Gst = dto.Gst,
            Quantity = dto.Quantity,
            SellerGSTIN = dto.SellerGSTIN,
            SellerName = dto.SellerName,
            Address = dto.Address,
            SellerInvoice = dto.SellerInvoice,
            PurchaseDate = dto.PurchaseDate,
            CreatedDate = DateTime.UtcNow
        };

        var product = await GetProductAsync(dto.ProductId, cancellationToken);
        product.Quantity += dto.Quantity;

        await _unitOfWork.LineItems.AddAsync(lineItem, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LineItemDto>(lineItem);
    }

    public async Task<IReadOnlyList<LineItemDto>> CreateBulkAsync(IEnumerable<CreateLineItemDto> dtos, CancellationToken cancellationToken = default)
    {
        var bulkDtos = dtos?.ToList() ?? new List<CreateLineItemDto>();
        if (!bulkDtos.Any())
        {
            return Array.Empty<LineItemDto>();
        }

        var createdItems = new List<LineItem>();
        foreach (var dto in bulkDtos)
        {
            var lineItem = new LineItem
            {
                Id = Guid.NewGuid(),
                ProductId = dto.ProductId,
                PurchasePrice = dto.PurchasePrice,
                Gst = dto.Gst,
                Quantity = dto.Quantity,
                SellerGSTIN = dto.SellerGSTIN,
                SellerName = dto.SellerName,
                Address = dto.Address,
                SellerInvoice = dto.SellerInvoice,
                PurchaseDate = dto.PurchaseDate,
                CreatedDate = DateTime.UtcNow
            };

            var product = await GetProductAsync(dto.ProductId, cancellationToken);
            product.Quantity += dto.Quantity;

            createdItems.Add(lineItem);
            await _unitOfWork.LineItems.AddAsync(lineItem, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<LineItemDto>>(createdItems);
    }

    public async Task<LineItemDto> UpdateAsync(Guid id, UpdateLineItemDto dto, CancellationToken cancellationToken = default)
    {
        var lineItem = await _unitOfWork.LineItems.FindAsync(x => x.Id == id, cancellationToken);
        var existingItem = lineItem.FirstOrDefault();
        
        if (existingItem == null)
        {
            throw new NotFoundException($"LineItem with id {id} was not found.");
        }

        if (existingItem.ProductId != dto.ProductId)
        {
            var originalProduct = await GetProductAsync(existingItem.ProductId, cancellationToken);
            var newProduct = await GetProductAsync(dto.ProductId, cancellationToken);

            originalProduct.Quantity -= existingItem.Quantity;
            newProduct.Quantity += dto.Quantity;
        }
        else
        {
            var product = await GetProductAsync(dto.ProductId, cancellationToken);
            product.Quantity += CalculateQuantityDelta(existingItem.Quantity, dto.Quantity);
        }

        existingItem.ProductId = dto.ProductId;
        existingItem.PurchasePrice = dto.PurchasePrice;
        existingItem.Gst = dto.Gst;
        existingItem.Quantity = dto.Quantity;
        existingItem.SellerGSTIN = dto.SellerGSTIN;
        existingItem.SellerName = dto.SellerName;
        existingItem.Address = dto.Address;
        existingItem.SellerInvoice = dto.SellerInvoice;
        existingItem.PurchaseDate = dto.PurchaseDate;

        await _unitOfWork.LineItems.UpdateAsync(existingItem, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LineItemDto>(existingItem);
    }

    public async Task<IReadOnlyList<LineItemDto>> UpdateBulkAsync(IEnumerable<UpdateLineItemBulkDto> dtos, CancellationToken cancellationToken = default)
    {
        var bulkDtos = dtos?.ToList() ?? new List<UpdateLineItemBulkDto>();
        if (!bulkDtos.Any())
        {
            return Array.Empty<LineItemDto>();
        }

        var ids = bulkDtos.Select(x => x.Id).ToList();
        var lineItems = await _unitOfWork.LineItems.FindAsync(x => ids.Contains(x.Id), cancellationToken);
        var existingItems = lineItems.ToDictionary(x => x.Id, x => x);

        foreach (var dto in bulkDtos)
        {
            if (!existingItems.TryGetValue(dto.Id, out var existingItem))
            {
                throw new NotFoundException($"LineItem with id {dto.Id} was not found.");
            }

            if (existingItem.ProductId != dto.ProductId)
            {
                var oldProduct = await GetProductAsync(existingItem.ProductId, cancellationToken);
                var newProduct = await GetProductAsync(dto.ProductId, cancellationToken);

                oldProduct.Quantity -= existingItem.Quantity;
                newProduct.Quantity += dto.Quantity;
            }
            else
            {
                var product = await GetProductAsync(dto.ProductId, cancellationToken);
                product.Quantity += CalculateQuantityDelta(existingItem.Quantity, dto.Quantity);
            }

            existingItem.ProductId = dto.ProductId;
            existingItem.PurchasePrice = dto.PurchasePrice;
            existingItem.Gst = dto.Gst;
            existingItem.Quantity = dto.Quantity;
            existingItem.SellerGSTIN = dto.SellerGSTIN;
            existingItem.SellerName = dto.SellerName;
            existingItem.Address = dto.Address;
            existingItem.SellerInvoice = dto.SellerInvoice;
            existingItem.PurchaseDate = dto.PurchaseDate;

            await _unitOfWork.LineItems.UpdateAsync(existingItem, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<LineItemDto>>(existingItems.Values);
    }

    // public async Task<IReadOnlyList<LineItemDto>> SellProductAsync(SellProductDto dto, CancellationToken cancellationToken = default)
    // {
    //     if (dto.Quantity <= 0)
    //     {
    //         throw new BusinessRuleException("Insufficient stock");
    //     }

    //     await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
    //     try
    //     {
    //         var product = await GetProductAsync(dto.ProductId, cancellationToken);
    //         var lineItems = (await _unitOfWork.LineItems.GetByProductIdForUpdateAsync(dto.ProductId, cancellationToken))
    //             .Where(x => x.Quantity > 0)
    //             .OrderBy(x => x.CreatedDate)
    //             .ToList();

    //         var availableQuantity = lineItems.Sum(x => x.Quantity);
    //         if (availableQuantity < dto.Quantity)
    //         {
    //             throw new BusinessRuleException("Insufficient stock");
    //         }

    //         var remainingQuantity = dto.Quantity;
    //         foreach (var lineItem in lineItems)
    //         {
    //             if (remainingQuantity <= 0)
    //             {
    //                 break;
    //             }

    //             var deduct = Math.Min(lineItem.Quantity, remainingQuantity);
    //             lineItem.Quantity -= deduct;
    //             remainingQuantity -= deduct;
    //             await _unitOfWork.LineItems.UpdateAsync(lineItem, cancellationToken);
    //         }

    //         product.Quantity -= dto.Quantity;
    //         await _unitOfWork.SaveChangesAsync(cancellationToken);
    //         await _unitOfWork.CommitTransactionAsync(cancellationToken);

    //         return _mapper.Map<IReadOnlyList<LineItemDto>>(lineItems);
    //     }
    //     catch
    //     {
    //         await _unitOfWork.RollbackTransactionAsync(cancellationToken);
    //         throw;
    //     }
    // }

    public async Task<IReadOnlyList<LineItemDto>> SellProductsAsync(IEnumerable<SellProductDto> dtos, CancellationToken cancellationToken = default)
    {
        var sellDtos = dtos?.ToList() ?? new List<SellProductDto>();
        if (!sellDtos.Any())
        {
            return Array.Empty<LineItemDto>();
        }

        await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        try
        {
            var updatedLineItems = new List<LineItem>();
            var groupedByProduct = sellDtos.GroupBy(x => x.ProductId);

            foreach (var productGroup in groupedByProduct)
            {
                var productId = productGroup.Key;
                var totalRequestQuantity = productGroup.Sum(x => x.Quantity);
                if (totalRequestQuantity <= 0)
                {
                    throw new BusinessRuleException("Insufficient stock");
                }

                var product = await GetProductAsync(productId, cancellationToken);
                var lineItems = (await _unitOfWork.LineItems.GetByProductIdForUpdateAsync(productId, cancellationToken))
                    .Where(x => x.Quantity > 0)
                    .OrderBy(x => x.CreatedDate)
                    .ToList();

                var availableQuantity = lineItems.Sum(x => x.Quantity);
                if (availableQuantity < totalRequestQuantity)
                {
                    throw new BusinessRuleException("Insufficient stock");
                }

                var remainingQuantity = totalRequestQuantity;
                foreach (var lineItem in lineItems)
                {
                    if (remainingQuantity <= 0)
                    {
                        break;
                    }

                    var deduct = Math.Min(lineItem.Quantity, remainingQuantity);
                    lineItem.Quantity -= deduct;
                    remainingQuantity -= deduct;
                    await _unitOfWork.LineItems.UpdateAsync(lineItem, cancellationToken);
                    updatedLineItems.Add(lineItem);
                }

                product.Quantity -= totalRequestQuantity;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return _mapper.Map<IReadOnlyList<LineItemDto>>(updatedLineItems);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<IReadOnlyList<Guid>> DeleteBulkAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        var bulkIds = ids?.ToList() ?? new List<Guid>();
        if (!bulkIds.Any())
        {
            return Array.Empty<Guid>();
        }

        var lineItems = await _unitOfWork.LineItems.FindAsync(x => bulkIds.Contains(x.Id), cancellationToken);
        var existingItems = lineItems.ToDictionary(x => x.Id, x => x);

        foreach (var id in bulkIds)
        {
            if (!existingItems.TryGetValue(id, out var existingItem))
            {
                throw new NotFoundException($"LineItem with id {id} was not found.");
            }

            var product = await GetProductAsync(existingItem.ProductId, cancellationToken);
            product.Quantity -= existingItem.Quantity;
            await _unitOfWork.LineItems.DeleteAsync(existingItem, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return existingItems.Keys.ToList();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var lineItem = await _unitOfWork.LineItems.FindAsync(x => x.Id == id, cancellationToken);
        var existingItem = lineItem.FirstOrDefault();
        
        if (existingItem == null)
        {
            throw new NotFoundException($"LineItem with id {id} was not found.");
        }

        var product = await GetProductAsync(existingItem.ProductId, cancellationToken);
        product.Quantity -= existingItem.Quantity;

        await _unitOfWork.LineItems.DeleteAsync(existingItem, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
