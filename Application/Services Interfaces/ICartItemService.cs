using Application.DTO.CartItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services_Interfaces
{
    public interface ICartItemService
    {
        Task<CartItemResponse> CreateCartItemAsync(Guid cartId, CreateCartItemRequest request);
        Task<CartItemResponse> GetCartItemByIdAsync(Guid cartItemId);
        Task<CartItemResponse> UpdateCartItemAsync(Guid cartItemId, UpdateCartItemRequest request);
    }
}
