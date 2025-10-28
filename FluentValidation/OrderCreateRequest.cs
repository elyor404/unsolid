using FluentValidation;
using Unsolid.Models;

namespace Unsolid.FluentValidation
{
    public class OrderCreateRequestValidator : AbstractValidator<OrderCreateRequest>
    {
        public OrderCreateRequestValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("Customer ID is required");

            RuleForEach(x => x.Items)
                .SetValidator(new OrderItemValidator());

            RuleFor(x => x.ShippingAddress)
                .NotNull()
                .WithMessage("Shipping address is required");
        }
    }
}