using AutoMapper;
using ShopManagementSystem.Application.DTOs.Bills;
using ShopManagementSystem.Application.Exceptions;
using ShopManagementSystem.Application.Interfaces;
using ShopManagementSystem.Domain.Entities;
using ShopManagementSystem.Domain.Interfaces;

namespace ShopManagementSystem.Application.Services;

public class BillService : IBillService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BillService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BillDto> CreateAsync(CreateBillDto dto, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(dto.CustomerId, cancellationToken)
            ?? throw new NotFoundException($"Customer with id {dto.CustomerId} was not found.");

        if (dto.Items == null || dto.Items.Count == 0)
        {
            throw new ValidationException("At least one bill item is required.");
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var billNumber = await _unitOfWork.Bills.GenerateBillNumberAsync(cancellationToken);
            var billItems = new List<BillItem>();
            decimal subTotal = 0;
            decimal gstAmount = 0;

            foreach (var itemDto in dto.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId, cancellationToken)
                    ?? throw new NotFoundException($"Product with id {itemDto.ProductId} was not found.");

                if (product.Quantity < itemDto.Quantity)
                {
                    throw new BusinessRuleException(
                        $"Insufficient stock for product '{product.ProductName}'. Available: {product.Quantity}, Requested: {itemDto.Quantity}.");
                }

                var lineSubTotal = product.CostPrice * itemDto.Quantity;
                var lineGst = lineSubTotal * (product.Gst / 100);
                var lineTotal = lineSubTotal + lineGst;

                subTotal += lineSubTotal;
                gstAmount += lineGst;

                billItems.Add(new BillItem
                {
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.CostPrice,
                    Gst = product.Gst,
                    LineTotal = lineTotal
                });

                product.Quantity -= itemDto.Quantity;
                await _unitOfWork.Products.UpdateAsync(product, cancellationToken);
            }

            var bill = new Bill
            {
                BillNumber = billNumber,
                CustomerId = customer.Id,
                BillDate = DateTime.UtcNow,
                SubTotal = subTotal,
                GstAmount = gstAmount,
                GrandTotal = subTotal + gstAmount,
                BillItems = billItems
            };

            await _unitOfWork.Bills.AddAsync(bill, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var createdBill = await _unitOfWork.Bills.GetByIdWithDetailsAsync(bill.Id, cancellationToken)
                ?? throw new NotFoundException("Bill was created but could not be retrieved.");

            return _mapper.Map<BillDto>(createdBill);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<IReadOnlyList<BillDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var bills = await _unitOfWork.Bills.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<BillDto>>(bills);
    }

    public async Task<BillDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var bill = await _unitOfWork.Bills.GetByIdWithDetailsAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Bill with id {id} was not found.");

        return _mapper.Map<BillDto>(bill);
    }

    public async Task<IReadOnlyList<BillDto>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
    {
        _ = await _unitOfWork.Customers.GetByIdAsync(customerId, cancellationToken)
            ?? throw new NotFoundException($"Customer with id {customerId} was not found.");

        var bills = await _unitOfWork.Bills.GetByCustomerIdAsync(customerId, cancellationToken);
        return _mapper.Map<IReadOnlyList<BillDto>>(bills);
    }
}
