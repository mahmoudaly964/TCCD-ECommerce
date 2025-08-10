using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public  async Task<List<Product>> GetAllAsync(
            Expression<Func<Product, bool>>? predicate = null,
            int? pageNumber = 1,
            int? pageSize = 5,
            bool tracking = true)
        {
            IQueryable<Product> query = _db.Products.Include(p => p.Category);

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

        public  async Task<Product?> GetAsync(Expression<Func<Product, bool>> predicate, bool tracking = true)
        {
            IQueryable<Product> query = _db.Products.Include(p => p.Category);

            if (!tracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(predicate);
        }
    }
}
