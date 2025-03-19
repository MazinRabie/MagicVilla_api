using MagicVilla_WebApi.DataStore;
using MagicVilla_WebApi.Models;
using MagicVilla_WebApi.Repository.IRepository;

namespace MagicVilla_WebApi.Repository
{
    public class VillaRepo : Repository<Villa>, IVillaRepo
    {
        private readonly ApplicationDbContext _context;
        public VillaRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Villa> Update(Villa entity)
        {
            entity.UpdatedDate = DateTime.Now;
            var villa = _context.Villas.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
