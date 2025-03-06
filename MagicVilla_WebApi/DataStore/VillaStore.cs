using MagicVilla_WebApi.Models.DTOs;

namespace MagicVilla_WebApi.DataStore
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>()
            {
                new VillaDTO() { Id = 1, Name = "summer" ,sqft=200,Occupancy=4},
                new VillaDTO() { Id = 2, Name = "winter",sqft=200,Occupancy=4 }
            };
    }
}
