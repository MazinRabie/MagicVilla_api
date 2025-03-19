using MagicVilla_WebApi.DataStore;
using MagicVilla_WebApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_WebApi.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbset;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbset = _context.Set<T>();
        }
        public async Task Create(T entity)
        {
            await _dbset.AddAsync(entity);
            await Save();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null)
        {
            List<T> entites;
            if (filter == null)
            { entites = await _dbset.ToListAsync(); }
            else
            {

                entites = await _dbset.Where(filter).ToListAsync();
            }
            return entites;
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression, bool tracked = true)
        {
            T? entity;
            if (tracked)
            {

                entity = await _dbset.FirstOrDefaultAsync(expression);

            }

            else { entity = await _dbset.AsNoTracking().FirstOrDefaultAsync(expression); }
            return entity;
        }

        public async Task Remove(T entity)
        {
            _dbset.Remove(entity);
            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();

        }

    }
}
