using cqrs_mediatr.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace cqrs_mediatr.Features.Products.Commands.Create
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        private readonly AppDbContext _dbContext;
        public CreateProductCommandValidator(AppDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Description).NotEmpty();
            RuleFor(p => p.Name).NotEmpty().MinimumLength(4).MustAsync(BeUniqueName).WithMessage("The product name already exists."); ;
            RuleFor(p => p.Price).GreaterThan(0);
            
        }

        private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            var isNameExists = await _dbContext.Products.AnyAsync(p => p.Name == name, cancellationToken); 
            return !isNameExists;
        }
    }
}
