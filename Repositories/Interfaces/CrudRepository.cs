using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models;
using System.Linq.Expressions;
using Serilog;
namespace RestaurantManagement.Repositories.Interfaces
{
    public class CrudRepository<T> : ICrudRepository<T> where T : class
    {
        private RestaurantManagementContext _context;
        private readonly Serilog.ILogger _logger;
        private DbSet<T> _entities;

        public CrudRepository(RestaurantManagementContext context, Serilog.ILogger logger)
        {
            _context = context;
            _logger = logger;
            _entities = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>> include = null)
        {
            try
            {
                var query = _entities.AsQueryable();
                if (include != null)
                {
                    query = query.Include(include);
                }
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }

        public async Task<bool> AddEntityAsync(T entity)
        {
            try
            {
                _entities.Add(entity);
                var result = await _context.SaveChangesAsync();
                return result > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }
        public async Task<bool> UpdateEntityAsync(T entity)
        {
            try
            {
                _entities.Update(entity);
                var result = await _context.SaveChangesAsync();
                return result > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }


        public async Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> include = null)
        {
            try
            {
                var query = _entities.AsQueryable();
                if (include != null)
                {
                    query = query.Include(include);
                }
                return await query?.FirstOrDefaultAsync(filter);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }

        public async Task<List<T>> GetListByFilterAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> include = null)
        {
            try
            {
                var query = _entities.AsQueryable();
                if (include != null)
                {
                    query = query.Include(include);
                }
                return await query?.Where(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }


    }
}
