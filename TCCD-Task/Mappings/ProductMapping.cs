using Application.DTO.Products;
using AutoMapper;
using Domain.Entities;

namespace TCCD_Task.Mappings
{
    public class ProductMapping : Profile
    {
        public ProductMapping() 
        {
            CreateMap<Product, CreateProductRequest>().ReverseMap();
            
            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "Unknown Category"));
                
            CreateMap<Product, UpdateProductRequest>().ReverseMap();
        }
    }
}
