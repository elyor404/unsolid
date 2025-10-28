using FluentValidation;
using Unsolid.Models;

namespace Unsolid.FluentValidation
{
    public class OrderItemValidator : AbstractValidator<OrderItemRequest>
    {
        public OrderItemValidator()
        {
            RuleFor(x => x.ProductId)
                .NotNull().WithMessage("You doesn't enter product id")
                .GreaterThan(0)
                .WithMessage("Product ID must be greater than 0");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be at least 1");
        }
    }
}