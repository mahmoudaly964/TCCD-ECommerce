using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        private readonly ApplicationDbContext _db;

        public CartItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        
        public  async Task<List<CartItem>> GetAllAsync(
            Expression<Func<CartItem, bool>>? predicate = null, 
            int? pageNumber = null, 
            int? pageSize = null, 
            bool tracking = true)
        {
            IQueryable<CartItem> query = _db.CartItems.Include(ci => ci.Product);

            if (!tracking)
                query = query.AsNoTracking();

            if (predicate != null)
                query = query.Where(predicate);

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                int skip = (pageNumber.Value - 1) * pageSize.Value;
                query = query.Skip(skip).Take(pageSize.Value);
            }

            return await query.ToListAsync();
        }
        public  async Task<CartItem?> GetAsync(Expression<Func<CartItem, bool>> predicate, bool tracking = true)
        {
            IQueryable<CartItem> query = _db.CartItems
                .Include(ci => ci.Product)
                .ThenInclude(p => p.Category);

            if (!tracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(predicate);
        }
    }
}
