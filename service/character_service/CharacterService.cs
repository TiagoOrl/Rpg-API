
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

        public async Task<List<Character>> AddCharacter(Character character)
        {
            characters.Add(character);
            return characters;
        }

        public async Task<List<Character>> GetAllCharacters()
        {
            return characters;
        }

        public async Task<Character> GetCharacterById(int id)
        {
            var foundCharacter =  characters.FirstOrDefault(c => {
                return c.Id == id;
            });

            if (foundCharacter is null) {
                throw new Exception("Character Not found");
            }
            return foundCharacter;
        }
    }
}