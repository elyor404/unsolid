using FluentValidation;
using Unsolid.Models;

namespace Unsolid.FluentValidation
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Product name is required")
                .MinimumLength(3)
                .WithMessage("Product name must be at least 3 characters");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero")
                .LessThanOrEqualTo(100000).WithMessage("Price cannot exceed $100,000");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative");
        }
    }
}

