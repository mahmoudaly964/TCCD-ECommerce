using Application.DTO.Products;
using Application.Response;
using Application.Services_Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace TCCD_Task.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        protected APIResponse _response;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
            _response = new APIResponse();
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIResponse>> CreateProductAsync([FromBody] CreateProductRequest request)
        {
            try
            {
                if (request == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var product = await _productService.CreateProductAsync(request);
                _response.Result = product;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<APIResponse>> GetProductByIdAsync(Guid productId)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(productId);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found.", productId);
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Product not found.");
                    return NotFound(_response);
                }

                _response.Result = product;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;

                _logger.LogInformation("Retrieved product with ID: {ProductId}", productId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving product with ID: {ProductId}", productId);
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return _response;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<APIResponse>> GetAllProductsAsync(int pageSize=1,int pageNumber=3)
        {
            try
            {
                var products = await _productService.GetAllProductsAsync(pageNumber: pageNumber, pageSize: pageSize);
                _response.Result = products;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;

                _logger.LogInformation("Retrieved all products. Count: {Count}", products?.Count() ?? 0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all products.");
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.NotFound;
            }
            return _response;
        }

        [HttpPatch("{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateProductAsync(Guid productId, [FromBody] UpdateProductRequest request)
        {
            try
            {
                if (request == null)
                {
                    _logger.LogWarning("UpdateProductAsync was called with a null request.");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var updatedProduct = await _productService.UpdateProductAsync(productId, request);
                if (updatedProduct == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found for update.", productId);
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Product not found.");
                    return NotFound(_response);
                }

                _logger.LogInformation("Product with ID {ProductId} updated successfully.", productId);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.Result = updatedProduct;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product with ID: {ProductId}", productId);
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return _response;
        }

        [HttpDelete("{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIResponse>> DeleteProductAsync(Guid productId)
        {
            try
            {
                
                 await _productService.DeleteProductAsync(productId);
                var deletedProduct = await _productService.GetProductByIdAsync(productId);
                if (deletedProduct==null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found for deletion.", productId);
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Product not found.");
                    return NotFound(_response);
                }
                await _productService.DeleteProductAsync(productId);
                _logger.LogInformation("Product with ID {ProductId} deleted successfully.", productId);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting product with ID: {ProductId}", productId);
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return _response;
        }


    }
}
