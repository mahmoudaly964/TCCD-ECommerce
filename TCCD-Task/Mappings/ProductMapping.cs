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

            // Fix: Only map non-null values from UpdateProductRequest to Product
            CreateMap<UpdateProductRequest, Product>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                .ForMember(dest => dest.Price, opt => opt.Condition(src => src.Price != null))
                .ForMember(dest => dest.CategoryId, opt => opt.Condition(src => src.CategoryId != null))
                .ForMember(dest => dest.Quantity, opt => opt.Ignore()) 
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) 
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.Category, opt => opt.Ignore()); 
        }
    }
}