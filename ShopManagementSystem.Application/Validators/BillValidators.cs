using FluentValidation;
using ShopManagementSystem.Application.DTOs.Bills;

namespace ShopManagementSystem.Application.Validators;

public class CreateBillDtoValidator : AbstractValidator<CreateBillDto>
{
    public CreateBillDtoValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0).WithMessage("CustomerId is required.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one bill item is required.");

        RuleForEach(x => x.Items).SetValidator(new CreateBillItemDtoValidator());
    }
}

public class CreateBillItemDtoValidator : AbstractValidator<CreateBillItemDto>
{
    public CreateBillItemDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("ProductId is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
    }
}
