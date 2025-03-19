using MagicVilla_WebApi.Models;

namespace MagicVilla_WebApi.Repository.IRepository
{
    public interface IVillaNumberRepo : IRepository<VillaNumber>
    {
        Task<VillaNumber> Update(VillaNumber villaNumber);

    }
}
