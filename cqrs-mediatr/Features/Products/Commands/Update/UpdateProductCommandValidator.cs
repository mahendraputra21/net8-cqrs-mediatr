using cqrs_mediatr.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace cqrs_mediatr.Features.Products.Commands.Update
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        private readonly AppDbContext _dbContext;

        public UpdateProductCommandValidator(AppDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Description).NotEmpty();

            RuleFor(p => p.Name).NotEmpty()
                                .MinimumLength(4)
                                .MustAsync(BeUniqueName)
                                .WithMessage("The product name already exists.");

            RuleFor(p => p.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Product price must be greater than zero")
                .LessThanOrEqualTo(1500).WithMessage("Product price cannot be more than 1500");
        }

        private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            var isNameExists = await _dbContext.Products.AnyAsync(p => p.Name == name, cancellationToken);
            return !isNameExists;
        }
    }
}
