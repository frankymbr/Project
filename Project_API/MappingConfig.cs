using AutoMapper;
using Project_API.Models;
using Project_API.Models.Dto;

namespace Project_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDto>();
            CreateMap<VillaDto, Villa>();

            //otra forma de hacer lo anterior

            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();

			CreateMap<NumeroVilla, NumeroVillaDto>().ReverseMap();
			CreateMap<NumeroVilla, NumeroVillaCreateDto>().ReverseMap();
			CreateMap<NumeroVilla, NumeroVillaUpdateDto>().ReverseMap();
		}
    }
}
