using FluentValidation;
using Unsolid.Models;

namespace Unsolid.FluentValidation
{
    public class CustomerCreateRequestValidator : AbstractValidator<CustomerCreateRequest>
    {
        public CustomerCreateRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Invalid email for");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Phone is required")
                .MaximumLength(10)
                .Matches(@"^\d+$")
                .WithMessage("Invalid phone format");
        }
    }
}