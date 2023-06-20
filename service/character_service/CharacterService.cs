
using first_api.models;

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

        public async Task<ServiceResponse<List<Character>>> AddCharacter(Character character)
        {
            var response = new ServiceResponse<List<Character>>();
            characters.Add(character);
            response.Data = characters;
            return response;
        }

        public async Task<ServiceResponse<List<Character>>> GetAllCharacters()
        {
            var response = new ServiceResponse<List<Character>>();
            response.Data = characters;
            return response;
        }

        public async Task<ServiceResponse<Character>> GetCharacterById(int id)
        {
            ServiceResponse<Character> response = new ServiceResponse<Character>();
            var foundCharacter =  characters.FirstOrDefault(c => {
                return c.Id == id;
            });

            if (foundCharacter is null) {
                response.Success = false;
                response.Message = "character of id not found";
                return response;
            }

            response.Data = foundCharacter;
            return response;
        }
    }
}