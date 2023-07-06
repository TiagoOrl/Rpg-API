using first_api.models;
using first_api.DTO.character;
using first_api.DTO.weapon;
using AutoMapper;

namespace first_api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
            CreateMap<UpdateCharacterDto, Character>();
            CreateMap<Weapon, GetWeaponDTO>();
            CreateMap<AddWeaponDTO, Weapon>();

        }
    }
}