using MagicVilla_WebApi.Models;

namespace MagicVilla_WebApi.Repository.IRepository
{
    public interface IVillaRepo : IRepository<Villa>
    {
        Task<Villa> Update(Villa villa);
    }
}
