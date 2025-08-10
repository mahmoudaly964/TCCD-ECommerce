using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate, bool tracking = true);
        Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            int? pageNumber = 1,
            int? pageSize = 3,
            bool tracking = true);

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteByIdAsync(Guid id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }

}
