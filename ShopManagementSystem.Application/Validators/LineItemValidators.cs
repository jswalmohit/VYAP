using FluentValidation;
using ShopManagementSystem.Application.DTOs.LineItems;

namespace ShopManagementSystem.Application.Validators;

public class CreateLineItemDtoValidator : AbstractValidator<CreateLineItemDto>
{
    public CreateLineItemDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.PurchasePrice)
            .GreaterThanOrEqualTo(0).WithMessage("PurchasePrice must be greater than or equal to 0.");

        RuleFor(x => x.Gst)
            .GreaterThanOrEqualTo(0).LessThanOrEqualTo(100).WithMessage("Gst must be between 0 and 100.");

        RuleFor(x => x.PurchaseDate)
            .NotEmpty().WithMessage("PurchaseDate is required.");
    }
}

public class UpdateLineItemDtoValidator : AbstractValidator<UpdateLineItemDto>
{
    public UpdateLineItemDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.PurchasePrice)
            .GreaterThanOrEqualTo(0).WithMessage("PurchasePrice must be greater than or equal to 0.");

        RuleFor(x => x.Gst)
            .GreaterThanOrEqualTo(0).LessThanOrEqualTo(100).WithMessage("Gst must be between 0 and 100.");

        RuleFor(x => x.PurchaseDate)
            .NotEmpty().WithMessage("PurchaseDate is required.");
    }
}
