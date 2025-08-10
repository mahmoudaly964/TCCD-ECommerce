using Application.DTO.Categories;
using Application.Services_Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync(int? pageNumber, int? pageSize)
        {
            var categories = await _categoryRepository.GetAllAsync(predicate: null, pageNumber: pageNumber, pageSize: pageSize, tracking: false);
            return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
        }

        public async Task<CategoryResponse> GetByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetAsync(c => c.Id == id);
            if (category == null) return null;

            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request)
        {
            var category = _mapper.Map<Category>(request);
            category.Id = Guid.NewGuid();

            await _categoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync(); 
            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task<CategoryResponse> UpdateAsync(Guid id, UpdateCategoryRequest request)
        {
            var category = await _categoryRepository.GetAsync(c => c.Id == id);
            if (category == null) return null;

            _mapper.Map(request, category); 
            await _categoryRepository.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var category = await _categoryRepository.GetAsync(c => c.Id == id);
            if (category == null) return false;

            await _categoryRepository.DeleteByIdAsync(category.Id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
