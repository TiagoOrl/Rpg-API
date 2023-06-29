using Microsoft.AspNetCore.Mvc;
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

        [HttpPut("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserRegisterDto dto)
        {
            var bodyReponse = await authService.Login(dto.Username, dto.Password);
            if (!bodyReponse.Success)
            {
                switch (bodyReponse.StatusCode)
                {
                    case 404:
                        return NotFound(bodyReponse);
                    case 401:
                        return Unauthorized(bodyReponse);
                    default:
                        return NotFound(bodyReponse);
                }
            }

            return Ok(bodyReponse);
        }
    }
}