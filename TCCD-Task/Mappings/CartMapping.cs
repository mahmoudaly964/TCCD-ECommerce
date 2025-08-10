using Application.DTO.Cart;
using AutoMapper;
using Domain.Entities;

namespace TCCD_Task.Mappings
{
    public class CartMapping : Profile
    {
        public CartMapping()
        {
            CreateMap<Cart, CartResponse>().ReverseMap();
            CreateMap<Cart, CreateCartRequest>().ReverseMap();
        }
    }
}
