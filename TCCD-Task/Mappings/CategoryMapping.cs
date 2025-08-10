using Application.DTO.Categories;
using AutoMapper;
using Domain.Entities;

namespace TCCD_Task.Mappings
{
    public class CategoryMapping:Profile
    {
        public CategoryMapping() {
            CreateMap<Category, CreateCategoryRequest>().ReverseMap();
            CreateMap<Category, CategoryResponse>().ReverseMap();
            CreateMap<Category, UpdateCategoryRequest>().ReverseMap();


        }
    }
}
