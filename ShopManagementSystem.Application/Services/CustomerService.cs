using AutoMapper;
using ShopManagementSystem.Application.DTOs.Customers;
using ShopManagementSystem.Application.Exceptions;
using ShopManagementSystem.Application.Interfaces;
using ShopManagementSystem.Domain.Entities;
using ShopManagementSystem.Domain.Interfaces;

namespace ShopManagementSystem.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CustomerDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var customers = await _unitOfWork.Customers.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<CustomerDto>>(customers);
    }

    public async Task<CustomerDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Customer with id {id} was not found.");

        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<CustomerDto> GetByMobileNumberAsync(string mobileNumber, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Customers.GetByMobileNumberAsync(mobileNumber, cancellationToken)
            ?? throw new NotFoundException($"Customer with mobile number '{mobileNumber}' was not found.");

        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Customers.IsMobileNumberUniqueAsync(dto.MobileNumber, cancellationToken: cancellationToken))
        {
            throw new BusinessRuleException($"Mobile number '{dto.MobileNumber}' already exists.");
        }

        var customer = _mapper.Map<Customer>(dto);
        customer.CreatedDate = DateTime.UtcNow;

        await _unitOfWork.Customers.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<CustomerDto> UpdateAsync(int id, UpdateCustomerDto dto, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Customer with id {id} was not found.");

        if (!await _unitOfWork.Customers.IsMobileNumberUniqueAsync(dto.MobileNumber, id, cancellationToken))
        {
            throw new BusinessRuleException($"Mobile number '{dto.MobileNumber}' already exists.");
        }

        _mapper.Map(dto, customer);
        await _unitOfWork.Customers.UpdateAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Customer with id {id} was not found.");

        await _unitOfWork.Customers.DeleteAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
