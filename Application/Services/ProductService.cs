using Application.DTO.Products;
using Application.Services_Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository,IUnitOfWork unitOfWork,IMapper mapper)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductResponse> CreateProductAsync(CreateProductRequest request)
        {
            if (request == null)
            {
                return null;
                throw new ArgumentNullException("Request can't be empty");
            }

            var product = _mapper.Map<Product>(request);

            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductResponse>(product);
        }


        public async Task DeleteProductAsync(Guid productId)
        {
            var product = await _productRepository.GetAsync(p => p.Id == productId, true);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with Id {productId} not found.");
            }

            await _productRepository.DeleteByIdAsync(productId);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<ProductResponse>> GetAllProductsAsync(int? pageNumber, int? pageSize)
        {
            var products = await _productRepository.GetAllAsync(predicate :null,pageNumber: pageNumber,pageSize: pageSize,tracking: false);

            return _mapper.Map<List<ProductResponse>>(products);
        }

        public async Task<ProductResponse> GetProductByIdAsync(Guid productId)
        {
            var product = await _productRepository.GetAsync(p => p.Id == productId, false);
            if (product == null)
                return null;


            return _mapper.Map<ProductResponse>(product);
        }

        public async Task<ProductResponse> UpdateProductAsync(Guid productId, UpdateProductRequest request)
        {
            if (request == null)
            {
                return null;
                throw new ArgumentNullException(nameof(request));
            }

            var product = await _productRepository.GetAsync(p => p.Id == productId, tracking: true);
            if (product == null)
            {
                return null;

                throw new KeyNotFoundException($"Product with Id {productId} not found.");
            }

            _mapper.Map(request, product); 

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductResponse>(product);
        }
    }
}
