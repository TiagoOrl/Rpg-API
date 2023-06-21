using AutoMapper;
using first_api.models;
using first_api.DTO.character;

namespace first_api.service.character_service
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character> {
            new Character {Id = 1, Name = "Jeff", Class = RpgClass.Cleric },
            new Character {Id = 2, Name = "Ana", Class = RpgClass.Knight },
            new Character {Id = 3, Name = "James", Class = RpgClass.Mage },
            new Character {Id = 4, Name = "Clara", Class = RpgClass.Mage }
        };

        private readonly IMapper mapper; 

        public CharacterService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto addCharacterDto)
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();
            var newCharacter = this.mapper.Map<Character>(addCharacterDto);
            newCharacter.Id = characters.Max(i => i.Id) + 1;
            
            characters.Add(newCharacter);
            response.Data = characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();
            response.Data = characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            var foundCharacter =  characters.FirstOrDefault(c => {
                return c.Id == id;
            });

            if (foundCharacter is null) {
                response.Success = false;
                response.Message = "character of id not found";
                return response;
            }

            response.Data = this.mapper.Map<GetCharacterDto>(foundCharacter);
            return response;
        }
    }
}