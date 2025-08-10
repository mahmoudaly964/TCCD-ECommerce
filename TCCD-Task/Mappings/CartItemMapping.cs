using Application.DTO.CartItem;
using AutoMapper;
using Domain.Entities;

namespace TCCD_Task.Mappings
{
    public class CartItemMapping : Profile
    {
        public CartItemMapping()
        {
            CreateMap<CartItem, CartItemResponse>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : "Unknown Product"))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product != null ? src.Product.Price : 0));

            CreateMap<CreateCartItemRequest, CartItem>();
            CreateMap<UpdateCartItemRequest, CartItem>();
        }
    }
}
