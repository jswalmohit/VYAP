using FluentValidation;
using ShopManagementSystem.Application.DTOs.Products;

namespace ShopManagementSystem.Application.Validators;

public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.")
            .MaximumLength(50).WithMessage("ProductId must not exceed 50 characters.");

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("ProductName is required.")
            .MaximumLength(200).WithMessage("ProductName must not exceed 200 characters.");

        RuleFor(x => x.CostPrice)
            .GreaterThanOrEqualTo(0).WithMessage("CostPrice must be greater than or equal to 0.");

        RuleFor(x => x.Gst)
            .InclusiveBetween(0, 100).WithMessage("Gst must be between 0 and 100.");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal to 0.");

        RuleFor(x => x.PurchaseDate)
            .NotEmpty().WithMessage("PurchaseDate is required.");
    }
}

public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.")
            .MaximumLength(50).WithMessage("ProductId must not exceed 50 characters.");

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("ProductName is required.")
            .MaximumLength(200).WithMessage("ProductName must not exceed 200 characters.");

        RuleFor(x => x.CostPrice)
            .GreaterThanOrEqualTo(0).WithMessage("CostPrice must be greater than or equal to 0.");

        RuleFor(x => x.Gst)
            .InclusiveBetween(0, 100).WithMessage("Gst must be between 0 and 100.");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal to 0.");

        RuleFor(x => x.PurchaseDate)
            .NotEmpty().WithMessage("PurchaseDate is required.");
    }
}
