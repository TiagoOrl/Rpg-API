
using first_api.models;

namespace first_api.service.character_service
{
    public interface ICharacterService
    {
        List<Character> GetAllCharacters();
        Character GetCharacterById(int id);
        List<Character> AddCharacter(Character character);
    }
}