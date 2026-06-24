using AutoMapper;
using ShopManagementSystem.Application.DTOs.Customers;
using ShopManagementSystem.Application.DTOs.LineItems;
using ShopManagementSystem.Application.DTOs.Products;
using ShopManagementSystem.Application.DTOs.ProductTypes;
using ShopManagementSystem.Application.DTOs.Sales;
using ShopManagementSystem.Domain.Entities;

namespace ShopManagementSystem.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();

        CreateMap<ProductType, ProductTypeDto>();
        CreateMap<CreateProductTypeDto, ProductType>();
        CreateMap<UpdateProductTypeDto, ProductType>();

        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>();

        CreateMap<LineItem, LineItemDto>();
        CreateMap<CreateLineItemDto, LineItem>();
        CreateMap<UpdateLineItemDto, LineItem>();

        CreateMap<Sale, SaleDto>();
        CreateMap<CreateSaleDto, Sale>();
        CreateMap<UpdateSaleDto, Sale>();
    }
}
