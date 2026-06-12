using AutoMapper;
using ShopManagementSystem.Application.DTOs.Sales;
using ShopManagementSystem.Application.Exceptions;
using ShopManagementSystem.Application.Interfaces;
using ShopManagementSystem.Domain.Entities;
using ShopManagementSystem.Domain.Interfaces;

public class SalesService : ISalesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SalesService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SaleDto> CreateAsync(CreateSaleDto dto, CancellationToken cancellationToken = default)
    {
        var sale = _mapper.Map<Sale>(dto);
        sale.CreatedDate = DateTime.UtcNow;
        sale.SaleDate = dto.SaleDate;

        await _unitOfWork.Sales.AddAsync(sale, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SaleDto>(sale);
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

    public async Task<IReadOnlyList<SaleDto>> GetByBillNoAsync(string billNo, CancellationToken cancellationToken = default)
    {
        var sales = await _unitOfWork.Sales.GetByBillNoAsync(billNo, cancellationToken);
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
