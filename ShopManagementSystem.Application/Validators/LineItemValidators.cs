using FluentValidation;
using ShopManagementSystem.Application.DTOs.LineItems;

namespace ShopManagementSystem.Application.Validators;

public class CreateLineItemDtoValidator : AbstractValidator<CreateLineItemDto>
{
    public CreateLineItemDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("ProductId must be a valid product ID.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.PurchasePrice)
            .GreaterThanOrEqualTo(0).WithMessage("PurchasePrice must be greater than or equal to 0.");

        RuleFor(x => x.Gst)
            .GreaterThanOrEqualTo(0).LessThanOrEqualTo(100).WithMessage("Gst must be between 0 and 100.");

        RuleFor(x => x.PurchaseDate)
            .NotEmpty().WithMessage("PurchaseDate is required.");

        RuleFor(x => x.SellerGSTIN)
            .MaximumLength(50).WithMessage("SellerGSTIN must not exceed 50 characters.")
            .Matches(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[A-Z0-9]{1}[Z]{1}[0-9A-Z]{1}$")
            .When(x => !string.IsNullOrEmpty(x.SellerGSTIN))
            .WithMessage("SellerGSTIN must be a valid GST number.");

        RuleFor(x => x.SellerName)
            .MaximumLength(200).WithMessage("SellerName must not exceed 200 characters.");
    }
}

public class UpdateLineItemDtoValidator : AbstractValidator<UpdateLineItemDto>
{
    public UpdateLineItemDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("ProductId must be a valid product ID.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.PurchasePrice)
            .GreaterThanOrEqualTo(0).WithMessage("PurchasePrice must be greater than or equal to 0.");

        RuleFor(x => x.Gst)
            .GreaterThanOrEqualTo(0).LessThanOrEqualTo(100).WithMessage("Gst must be between 0 and 100.");

        RuleFor(x => x.PurchaseDate)
            .NotEmpty().WithMessage("PurchaseDate is required.");

        RuleFor(x => x.SellerGSTIN)
            .MaximumLength(50).WithMessage("SellerGSTIN must not exceed 50 characters.")
            .Matches(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[A-Z0-9]{1}[Z]{1}[0-9A-Z]{1}$")
            .When(x => !string.IsNullOrEmpty(x.SellerGSTIN))
            .WithMessage("SellerGSTIN must be a valid GST number.");

        RuleFor(x => x.SellerName)
            .MaximumLength(200).WithMessage("SellerName must not exceed 200 characters.");
    }
}
