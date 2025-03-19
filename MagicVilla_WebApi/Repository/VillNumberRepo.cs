using MagicVilla_WebApi.DataStore;
using MagicVilla_WebApi.Models;
using MagicVilla_WebApi.Repository.IRepository;

namespace MagicVilla_WebApi.Repository
{
    public class VillaNumberRepo : Repository<VillaNumber>, IVillaNumberRepo
    {
        private readonly ApplicationDbContext context;

        public VillaNumberRepo(ApplicationDbContext context) : base(context)
        {
            this.context = context;

        }

        public async Task<VillaNumber> Update(VillaNumber villaNumber)
        {
            //var old = await context.VillaNumbers.FirstOrDefaultAsync(x => x.VillaNo == villaNumber.VillaNo);


            //old.SpecialDetails = villaNumber.SpecialDetails;
            villaNumber.UpdatedDate = DateTime.Now;
            context.VillaNumbers.Update(villaNumber);
            await context.SaveChangesAsync();
            return villaNumber;


        }
    }
}
