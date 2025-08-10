using Application.DTO.CartItem;
using Application.Response;
using Application.Services_Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace TCCD_Task.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;
        private readonly ILogger<CartItemsController> _logger;
        private readonly APIResponse _response;

        public CartItemsController(ICartItemService cartItemService, ILogger<CartItemsController> logger)
        {
            _cartItemService = cartItemService;
            _logger = logger;
            _response = new APIResponse();
        }

        [HttpPost("{cartId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateCartItemAsync(Guid cartId, [FromBody] CreateCartItemRequest request)
        {
            try
            {
                if (request == null)
                {
                    _logger.LogWarning("CreateCartItemRequest is null");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var cartItem = await _cartItemService.CreateCartItemAsync(cartId, request);
                _response.Result = cartItem;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating cart item for CartId {CartId}", cartId);
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpGet("{cartItemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCartItemByIdAsync(Guid cartItemId)
        {
            try
            {
                var cartItem = await _cartItemService.GetCartItemByIdAsync(cartItemId);

                if (cartItem == null)
                {
                    _logger.LogWarning("Cart item not found with Id {CartItemId}", cartItemId);
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = cartItem;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cart item {CartItemId}", cartItemId);
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPatch("{cartItemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateCartItemAsync(Guid cartItemId, [FromBody] UpdateCartItemRequest request)
        {
            try
            {
                if (request == null)
                {
                    _logger.LogWarning("UpdateCartItemRequest is null");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var updatedCartItem = await _cartItemService.UpdateCartItemAsync(cartItemId, request);

                if (updatedCartItem == null)
                {
                    _logger.LogWarning("Cart item not found for update with Id {CartItemId}", cartItemId);
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = updatedCartItem;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart item {CartItemId}", cartItemId);
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
    }
}
