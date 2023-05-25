using first_api.models;
using Microsoft.AspNetCore.Mvc;

namespace first_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService characterService;

        public CharacterController(ICharacterService characterService)
        {
            this.characterService = characterService;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<List<Character>>> GetAllChars() 
        {
            return Ok(await characterService.GetAllCharacters());
        }

        [HttpGet("get-single/{id}")]
        public async Task<ActionResult<Character>> GetOne(int id) 
        {
            return Ok(await characterService.GetCharacterById(id));
        }

        [HttpPost]
        public async Task<ActionResult<List<Character>>> AddCharacter(Character character) 
        {
            return Ok(await characterService.AddCharacter(character));
        }
    }
}