using Application.DTO.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services_Interfaces
{
    public interface IProductService
    {
        Task<ProductResponse> CreateProductAsync(CreateProductRequest request);
        Task<List<ProductResponse>> GetAllProductsAsync(int? pageNumber = 1, int? pageSize = 3);
        Task<ProductResponse> GetProductByIdAsync(Guid id);
        Task<ProductResponse> UpdateProductAsync(Guid id, UpdateProductRequest request);
        Task DeleteProductAsync(Guid id);
    }
}
