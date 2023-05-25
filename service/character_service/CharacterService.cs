
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

        public List<Character> AddCharacter(Character character)
        {
            characters.Add(character);
            return characters;
        }

        public List<Character> GetAllCharacters()
        {
            return characters;
        }

        public Character GetCharacterById(int id)
        {
            return characters.FirstOrDefault(c => {
                return c.Id == id;
            });
        }
    }
}