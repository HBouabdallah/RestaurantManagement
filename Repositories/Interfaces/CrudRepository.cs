using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models;
using System.Linq.Expressions;

namespace RestaurantManagement.Repositories.Interfaces
{
    public class CrudRepository<T> : ICrudRepository<T> where T : class
    {
        private RestaurantManagementContext _context;
        private DbSet<T> _entities;

        public CrudRepository(RestaurantManagementContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>> include = null)
        {
            var query = _entities.AsQueryable();
            if (include != null)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<bool> AddEntityAsync(T entity)
        {
            _entities.Add(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
        public async Task<bool> UpdateEntityAsync(T entity)
        {
            _entities.Update(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }


        public async Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> include = null)
        {
            var query = _entities.AsQueryable();
            if (include != null)
            {
                query = query.Include(include);
            }
            return await query?.FirstOrDefaultAsync(filter);
        }

        public async Task<List<T>> GetListByFilterAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> include = null)
        {
            var query = _entities.AsQueryable();
            if (include != null)
            {
                query = query.Include(include);
            }

            return await query?.Where(filter).ToListAsync();
        }


    }
}
