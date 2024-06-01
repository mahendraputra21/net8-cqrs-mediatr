using AutoMapper;
using cqrs_mediatr.Models;
using cqrs_mediatr.Repositories;
using MediatR;

namespace cqrs_mediatr.Features.Products.Queries.List
{
    public class ListProductsQueryHandler : IRequestHandler<ListProductsQuery, List<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ListProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> Handle(ListProductsQuery request, CancellationToken cancellationToken)
        {
           var products = await _productRepository.GetAllProductsAsync(cancellationToken);
           return _mapper.Map<List<ProductDto>>(products);
        }
    }
}
