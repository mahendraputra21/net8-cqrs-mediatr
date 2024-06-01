using FluentValidation;

namespace cqrs_mediatr.Features.Products.Commands.Update
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Description).NotEmpty();

            RuleFor(p => p.Name).NotEmpty()
                                .MinimumLength(4);

            RuleFor(p => p.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Product price must be greater than zero")
                .LessThanOrEqualTo(1500).WithMessage("Product price cannot be more than 1500");
        }
    }
}
