using Microsoft.AspNetCore.Mvc;
using first_api.service.user_service;
using first_api.DTO.user;
using first_api.models;


namespace first_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authService;

        public AuthController(IAuthRepository authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto userDto)
        {
            var bodyReponse = await authService.Register(userDto, userDto.Password);
            if (!bodyReponse.Success)
            {
                return Conflict(bodyReponse);
            }

            return Ok(bodyReponse);
        }
    }
}