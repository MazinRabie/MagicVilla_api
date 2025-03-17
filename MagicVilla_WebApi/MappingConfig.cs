using AutoMapper;
using MagicVilla_WebApi.Models;
using MagicVilla_WebApi.Models.DTOs;

namespace MagicVilla_WebApi
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDTO>().ReverseMap();
            CreateMap<Villa, VillaDTOCreate>().ReverseMap();
            CreateMap<Villa, VillaDTOUpdate>().ReverseMap();

        }
    }
}
