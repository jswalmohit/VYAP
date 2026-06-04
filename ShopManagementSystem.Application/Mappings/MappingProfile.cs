using AutoMapper;
using ShopManagementSystem.Application.DTOs.Bills;
using ShopManagementSystem.Application.DTOs.Customers;
using ShopManagementSystem.Application.DTOs.LineItems;
using ShopManagementSystem.Application.DTOs.Products;
using ShopManagementSystem.Domain.Entities;

namespace ShopManagementSystem.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();

        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>();

        CreateMap<LineItem, LineItemDto>();
        CreateMap<CreateLineItemDto, LineItem>();
        CreateMap<UpdateLineItemDto, LineItem>();

        CreateMap<BillItem, BillItemDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName));

        CreateMap<Bill, BillDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.CustomerName));
    }
}
