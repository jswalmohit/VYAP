using FluentValidation;
using ShopManagementSystem.Application.DTOs.Customers;

namespace ShopManagementSystem.Application.Validators;

public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
{
    public CreateCustomerDtoValidator()
    {
        RuleFor(x => x.CustomerId)
            .MaximumLength(50).WithMessage("CustomerId must not exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.CustomerId));

        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("CustomerName is required.")
            .MaximumLength(200).WithMessage("CustomerName must not exceed 200 characters.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber is required.")
            .Matches(@"^\d{10}$").WithMessage("PhoneNumber must be a 10-digit number.");

        RuleFor(x => x.AddressLine1)
            .MaximumLength(500).WithMessage("AddressLine1 must not exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.AddressLine1));

        RuleFor(x => x.AddressLine2)
            .MaximumLength(500).WithMessage("AddressLine2 must not exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.AddressLine2));

        RuleFor(x => x.AddressLine3)
            .MaximumLength(500).WithMessage("AddressLine3 must not exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.AddressLine3));

        RuleFor(x => x.District)
            .MaximumLength(200).WithMessage("District must not exceed 200 characters.")
            .When(x => !string.IsNullOrEmpty(x.District));

        RuleFor(x => x.State)
            .MaximumLength(200).WithMessage("State must not exceed 200 characters.")
            .When(x => !string.IsNullOrEmpty(x.State));

        RuleFor(x => x.Pincode)
            .GreaterThan(0).WithMessage("Pincode must be a positive number.")
            .When(x => x.Pincode.HasValue);
    }
}

public class UpdateCustomerDtoValidator : AbstractValidator<UpdateCustomerDto>
{
    public UpdateCustomerDtoValidator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("CustomerName is required.")
            .MaximumLength(200).WithMessage("CustomerName must not exceed 200 characters.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber is required.")
            .Matches(@"^\d{10}$").WithMessage("PhoneNumber must be a 10-digit number.");

        RuleFor(x => x.AddressLine1)
            .MaximumLength(500).WithMessage("AddressLine1 must not exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.AddressLine1));

        RuleFor(x => x.AddressLine2)
            .MaximumLength(500).WithMessage("AddressLine2 must not exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.AddressLine2));

        RuleFor(x => x.AddressLine3)
            .MaximumLength(500).WithMessage("AddressLine3 must not exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.AddressLine3));

        RuleFor(x => x.District)
            .MaximumLength(200).WithMessage("District must not exceed 200 characters.")
            .When(x => !string.IsNullOrEmpty(x.District));

        RuleFor(x => x.State)
            .MaximumLength(200).WithMessage("State must not exceed 200 characters.")
            .When(x => !string.IsNullOrEmpty(x.State));

        RuleFor(x => x.Pincode)
            .GreaterThan(0).WithMessage("Pincode must be a positive number.")
            .When(x => x.Pincode.HasValue);
    }
}
