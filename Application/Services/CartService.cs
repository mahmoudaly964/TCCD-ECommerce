using Application.DTO.Cart;
using Application.DTO.CartItem;
using Application.DTO.Categories;
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
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        ILogger<CartService> _logger;
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        public CartService(ICartRepository cartRepository, ICartItemRepository cartItemRepository,
            ILogger<CartService> logger, IMapper mapper, IUnitOfWork unitOfWork) { 
            _cartItemRepository = cartItemRepository;
            _cartRepository = cartRepository;
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;

        }
        public async Task<CartResponse> CreateCartAsync(CreateCartRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException( "CreateCartRequest cannot be null");
            }
            var existingCart = await _cartRepository.GetCartByUserIdAsync(request.UserId, tracking: false);
            if (existingCart != null)
            {
                _logger.LogWarning("User {UserId} already has a cart with ID: {CartId}", request.UserId, existingCart.Id);
                throw new InvalidOperationException("User already has a cart");
            }
            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow,
                Items = new List<CartItem>()
            };
            await _cartRepository.AddAsync(cart);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CartResponse>(cart);
        }

        public async Task<bool> DeleteCartAsync(Guid cartId)
        {
            if (cartId == Guid.Empty)
            {
                throw new ArgumentException("Cart ID cannot be empty");
            }
            var cart = await _cartRepository.GetAsync(c => c.Id == cartId, tracking: true);
            if (cart == null)
            {
                return false;
            }
            await _cartRepository.DeleteByIdAsync(cartId);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<CartItemResponse>> GetCartItemsAsync(Guid cartId)
        {
            if (cartId == Guid.Empty)
            {
                throw new ArgumentException("Cart ID cannot be empty");
            }

            var cartItems = await _cartItemRepository.GetAllAsync(item => item.CartId == cartId, tracking: false);
            return _mapper.Map<List<CartItemResponse>>(cartItems);
        }
    }
}
