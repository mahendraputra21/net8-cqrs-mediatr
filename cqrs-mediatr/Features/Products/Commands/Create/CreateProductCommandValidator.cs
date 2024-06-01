using cqrs_mediatr.Repositories;
using FluentValidation;

namespace cqrs_mediatr.Features.Products.Commands.Create
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
       private readonly IProductRepository _productRepository;
        public CreateProductCommandValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;

            RuleFor(x => x.Description).NotEmpty();

            RuleFor(p => p.Name).NotEmpty()
                                .MinimumLength(4)
                                .MustAsync(IsUniqueName)
                                .WithMessage("The product name already exists.");

            RuleFor(p => p.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Product price must be greater than zero")
                .LessThanOrEqualTo(1500).WithMessage("Product price cannot be more than 1500");
            
        }

        private async Task<bool> IsUniqueName(string name, CancellationToken cancellationToken)
        {
            var isNameExists = await _productRepository.IsUniqueProductNameAsync(name, cancellationToken);
            return !isNameExists;
        }
    }
}
