using DiamondShop.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShop.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly Prn231DiamondShopContext _context;

        public GenericRepository(Prn231DiamondShopContext context)
        {
            _context = context;
        }

        public IQueryable<T> Entities => _context.Set<T>();

        public Task<T> AddAsync(T TEntity)
        {
            _context.Add(TEntity);
            return Task.FromResult(TEntity);
        }

        public Task DeleteAsync(T TEntity)
        {
            _context.Remove(TEntity);
            return Task.CompletedTask;
        }

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<T?> FindOneAsync(Expression<Func<T, bool>> expression, bool hasTrackings = true)
        {
            return hasTrackings ? await _context.Set<T>().FirstOrDefaultAsync(expression)
                                : await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public Task UpdateAsync(T TEntity)
        {
            _context.Set<T>().Update(TEntity);
            return Task.CompletedTask;
        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().AnyAsync(expression);
        }

        public async Task AddRangeAsync(IEnumerable<T> Tentities)
        {
            await _context.Set<T>().AddRangeAsync(Tentities);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().CountAsync(expression);
        }

        public Task DeleteRangeAsync(IEnumerable<T> TEntities)
        {
            _context.Set<T>().RemoveRange(TEntities);
            return Task.CompletedTask;
        }
    }
}
