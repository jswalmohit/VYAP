using AutoMapper;
    using ShopManagementSystem.Application.DTOs.LineItems;
using ShopManagementSystem.Application.DTOs.Sales;
using ShopManagementSystem.Application.Exceptions;
using ShopManagementSystem.Application.Interfaces;
using ShopManagementSystem.Domain.Entities;
using ShopManagementSystem.Domain.Interfaces;

public class SalesService : ISalesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IInvoiceNumberService _invoiceNumberService;
    private readonly ILineItemService _lineItemService;

    public SalesService(IUnitOfWork unitOfWork, IMapper mapper, IInvoiceNumberService invoiceNumberService, ILineItemService lineItemService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _invoiceNumberService = invoiceNumberService;
        _lineItemService = lineItemService;
    }

    public async Task<SaleDto> CreateAsync(CreateSaleDto dto, CancellationToken cancellationToken = default)
    {
        // Validate product list
        if (dto.ProductList == null || !dto.ProductList.Any())
        {
            throw new BusinessRuleException("ProductList cannot be empty.");
        }

        // Generate invoice number
        var invoiceNumber = _invoiceNumberService.GetOrGenerateInvoiceNumber();

        // Update inventory using LineItemService (handles transaction & quantity validation)
        await _lineItemService.SellProductsAsync(dto.ProductList, cancellationToken);

        try
        {
            // Create and save a sale record for each product in the list
            foreach (var product in dto.ProductList)
            {
                // Verify product exists
                var productEntity = await _unitOfWork.Products.GetByProductIdAsync(product.ProductId, cancellationToken);
                if (productEntity == null)
                {
                    throw new NotFoundException($"Product with id {product.ProductId} was not found.");
                }

                // Use provided USP or fallback to product's SalePrice
                var usp = product.USP ?? productEntity.SalePrice;

                var sale = new Sale
                {
                    ProductId = product.ProductId,
                    CustomerId = dto.CustomerId,
                    Quantity = product.Quantity,
                    USP = usp,
                    UpdatedDate = DateTime.UtcNow,
                    InvoiceNo = invoiceNumber,
                    CreatedDate = DateTime.UtcNow,
                    CGSTRate = product.CGSTRate,
                    SGSTRate = product.SGSTRate,
                    UpdatedBy = dto.UpdatedBy,
                    IPAddress = dto.IPAddress
                };

                await _unitOfWork.Sales.AddAsync(sale, cancellationToken);
                
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
        }

        return _mapper.Map<SaleDto>(new Sale { InvoiceNo = invoiceNumber });
    }

    public async Task<SaleDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var sale = await _unitOfWork.Sales.GetByIdAsync(id, cancellationToken);
        if (sale is null)
            throw new NotFoundException($"Sale with ID {id} not found.");

        return _mapper.Map<SaleDto>(sale);
    }

    public async Task<IReadOnlyList<SaleDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var sales = await _unitOfWork.Sales.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<SaleDto>>(sales);
    }

    public async Task<IReadOnlyList<SaleDto>> GetByProductIdAsync(string productId, CancellationToken cancellationToken = default)
    {
        var sales = await _unitOfWork.Sales.GetByProductIdAsync(productId, cancellationToken);
        return _mapper.Map<IReadOnlyList<SaleDto>>(sales);
    }

    public async Task<IReadOnlyList<SaleDto>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default)
    {
        var sales = await _unitOfWork.Sales.GetByCustomerIdAsync(customerId, cancellationToken);
        return _mapper.Map<IReadOnlyList<SaleDto>>(sales);
    }

    public async Task<IReadOnlyList<SaleDto>> GetByInvoiceNoAsync(string invoiceNo, CancellationToken cancellationToken = default)
    {
        var sales = await _unitOfWork.Sales.GetByInvoiceNoAsync(invoiceNo, cancellationToken);
        return _mapper.Map<IReadOnlyList<SaleDto>>(sales);
    }

    public async Task<SaleDto> UpdateAsync(UpdateSaleDto dto, CancellationToken cancellationToken = default)
    {
        var sale = await _unitOfWork.Sales.GetByIdAsync(dto.Id, cancellationToken);
        if (sale is null)
            throw new NotFoundException($"Sale with ID {dto.Id} not found.");

        _mapper.Map(dto, sale);
        await _unitOfWork.Sales.UpdateAsync(sale, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SaleDto>(sale);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var sale = await _unitOfWork.Sales.GetByIdAsync(id, cancellationToken);
        if (sale is null)
            throw new NotFoundException($"Sale with ID {id} not found.");

        await _unitOfWork.Sales.DeleteAsync(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

