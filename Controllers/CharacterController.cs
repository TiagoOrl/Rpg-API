using first_api.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using first_api.DTO.character;

namespace first_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService characterService;

        public CharacterController(ICharacterService characterService)
        {
            this.characterService = characterService;
        }

        [AllowAnonymous]
        [HttpGet("get-all")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> GetAllChars() 
        {
            return Ok(await characterService.GetAllCharacters());
        }

        [HttpGet("get-single/{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetOne(int id) 
        {
            var body = await characterService.GetCharacterById(id);
            if (!body.Success)
            {
                return NotFound(body);
            }
            return Ok(body);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto character) 
        {
            return Ok(await characterService.AddCharacter(character));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> UpdateCharacter(UpdateCharacterDto characterDto) 
        {
            var body = await characterService.UpdateCharacter(characterDto);
            if (!body.Success)
            {
                return NotFound(body);
            }
            return Ok(body);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var bodyResponse = await characterService.DeleteCharacter(id);
            if (!bodyResponse.Success)
            {
                return NotFound(bodyResponse);
            }

            return Ok(bodyResponse);
        }
    }
}