using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ShopManagementSystem.Application.Interfaces;
using ShopManagementSystem.Application.Mappings;
using ShopManagementSystem.Application.Services;

namespace ShopManagementSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ILineItemService, LineItemService>();
        services.AddScoped<ISalesService, SalesService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddSingleton<IInvoiceNumberService, InvoiceNumberService>();

        return services;
    }
}
