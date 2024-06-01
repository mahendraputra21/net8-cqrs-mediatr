using AutoMapper;
using cqrs_mediatr.Domain;
using cqrs_mediatr.Model;

namespace cqrs_mediatr.Behaviors
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to DTO mappings
            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Product.Description))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

            CreateMap<Cart, CartDto>();
        }
    }
}
