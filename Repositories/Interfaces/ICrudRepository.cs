using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RestaurantManagement.Repositories.Interfaces
{
    public interface ICrudRepository<T> where T:class
    {
        public  Task<T> GetByFilterAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>? include = null);
        public  Task<List<T>> GetListByFilterAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>? include = null);
        public  Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>>? include = null);
        public  Task<bool> AddEntityAsync(T entity);
        public Task<bool> UpdateEntityAsync(T entity);

    }
}
