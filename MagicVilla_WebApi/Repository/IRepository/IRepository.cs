using System.Linq.Expressions;

namespace MagicVilla_WebApi.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task Create(T entity);
        Task Remove(T entity);
        Task Save();

        Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null);
        Task<T> Get(Expression<Func<T, bool>> expression, bool tracked = true);
    }
}
