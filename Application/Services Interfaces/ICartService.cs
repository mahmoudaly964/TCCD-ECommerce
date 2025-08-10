using Application.DTO.Cart;
using Application.DTO.CartItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services_Interfaces
{
    public interface ICartService
    {
        Task<CartResponse> CreateCartAsync(CreateCartRequest request);
        Task<bool> DeleteCartAsync(Guid cartId);
        Task<List<CartItemResponse>> GetCartItemsAsync(Guid cartId);
    }
}
