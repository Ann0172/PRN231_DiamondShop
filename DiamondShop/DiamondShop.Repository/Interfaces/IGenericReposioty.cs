using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShop.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Entities { get; }
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task<int> CountAsync(Expression<Func<T, bool>> expression);
        Task AddRangeAsync(IEnumerable<T> Tentities);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> expression);

        Task<T?> FindOneAsync(Expression<Func<T, bool>> expression, bool hasTrackings = true);

        Task<T?> GetByIdAsync(Guid id);

        Task<List<T>> GetAllAsync();

        Task<T> AddAsync(T TEntity);

        Task UpdateAsync(T TEntity);

        Task DeleteAsync(T TEntity);

        Task DeleteRangeAsync(IEnumerable<T> TEntities);
    }
}
