using Application.DTO.CartItem;
using Application.Services_Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CartItemService> _logger;

        public CartItemService(
            ICartRepository cartRepository,
            IProductRepository productRepository,
            ICartItemRepository cartItemRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CartItemService> logger)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _cartItemRepository = cartItemRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CartItemResponse> CreateCartItemAsync(Guid cartId, CreateCartItemRequest request)
        {
            var cart = await _cartRepository.GetAsync(c => c.Id == cartId, tracking: true);
            if (cart == null)
            {
                throw new KeyNotFoundException("Cart not found");
            }

            var product = await _productRepository.GetAsync(p => p.Id == request.ProductId);
            if (product == null)
            {
                return null;
                //throw new KeyNotFoundException("Product not found");
            }

            // Check if product has enough quantity
            if (product.Quantity < request.Quantity)
            {
                throw new InvalidOperationException("Insufficient product quantity");
            }

            var cartItem = _mapper.Map<CartItem>(request);
            cartItem.CartId = cartId;
            cartItem.Id = Guid.NewGuid();

            await _cartItemRepository.AddAsync(cartItem);
            await _unitOfWork.SaveChangesAsync();

            // Retrieve the created cart item with Product included
            var createdCartItem = await _cartItemRepository.GetAsync(ci => ci.Id == cartItem.Id, false);


            return _mapper.Map<CartItemResponse>(createdCartItem);
            
        }

        public async Task<CartItemResponse> GetCartItemByIdAsync(Guid cartItemId)
        {
            var cartItem = await _cartItemRepository.GetAsync(ci => ci.Id == cartItemId, tracking: false);
            if (cartItem == null)
            {
                return null;
                //throw new KeyNotFoundException("Cart item not found");
            }

            return _mapper.Map<CartItemResponse>(cartItem);
        }

        public async Task<CartItemResponse> UpdateCartItemAsync(Guid cartItemId, UpdateCartItemRequest request)
        {

            var cartItem = await _cartItemRepository.GetAsync(ci => ci.Id == cartItemId, tracking: true);
            if (cartItem == null)
            {
                return null; 
                //throw new KeyNotFoundException("Cart item not found");
            }

            if (cartItem.Product != null && cartItem.Product.Quantity < request.Quantity)
            {
                    
                throw new InvalidOperationException("Insufficient product quantity");
            }

            _mapper.Map(request, cartItem);

            await _cartItemRepository.UpdateAsync(cartItem);
            await _unitOfWork.SaveChangesAsync();


            return _mapper.Map<CartItemResponse>(cartItem);
           
        }

        public async Task<List<CartItemResponse>> GetCartItemsByCartIdAsync(Guid cartId)
        {

            var cartItems = await _cartItemRepository.GetAllAsync(ci => ci.CartId == cartId, tracking: false);
            return _mapper.Map<List<CartItemResponse>>(cartItems);
            
        }

        public async Task DeleteCartItemAsync(Guid cartItemId)
        {

            var cartItem = await _cartItemRepository.GetAsync(ci => ci.Id == cartItemId, tracking: true);
            if (cartItem == null)
            {
                return; 
            }

            await _cartItemRepository.DeleteByIdAsync(cartItemId);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
