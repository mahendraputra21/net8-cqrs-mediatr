﻿using DewaEShop.Infrastructure.Persistence;
using DewaEShop.Infrastructure.Repositories;
using Mediator;

namespace DewaEShop.Application.Features.Products.Commands.Update
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async ValueTask<Guid> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.SelectProductByIdAsync(request.Id, cancellationToken);

            if (product == null)
                return Guid.Empty;

            product.UpdateProduct(request.Name, request.Description, request.Price);
            _productRepository.UpdateProductAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}