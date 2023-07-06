using first_api.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using first_api.DTO.weapon;
using first_api.service.weapon_service;

namespace first_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService weaponService;

        public WeaponController(IWeaponService weaponService)
        {
            this.weaponService = weaponService;
        }

        [HttpPost("add")]
        public async Task<ActionResult<ServiceResponse<GetWeaponDTO>>> AddWeaponToCharacter(AddWeaponDTO addWeaponDTO)
        {
            var bodyResponse = await weaponService.AddWeapon(addWeaponDTO);

            switch (bodyResponse.StatusCode)
            {
                case 422:
                    return UnprocessableEntity(bodyResponse);
                case 404:
                    return NotFound(bodyResponse);
                case 500:
                    return StatusCode(500, bodyResponse);
                default:
                    return Ok(bodyResponse);
            }
        }
    }
}