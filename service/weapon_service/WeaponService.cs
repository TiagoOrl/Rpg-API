using System.Security.Claims;
using AutoMapper;

using first_api.DTO.weapon;
using first_api.models;

namespace first_api.service.weapon_service
{
    public class WeaponService : IWeaponService
    {
        private readonly IMapper mapper; 
        private readonly DataContext dataContext;
        private readonly IHttpContextAccessor httpAcessor;
        public WeaponService(IMapper mapper, DataContext dataContext, IHttpContextAccessor httpAcessor)
        {
            this.httpAcessor = httpAcessor;
            this.mapper = mapper;
            this.dataContext = dataContext;
        }

        // gets the user id from the jwt passed in the http request header
        private int GetUserId() => int.Parse(httpAcessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<GetWeaponDTO>> AddWeapon(AddWeaponDTO addWeaponDTO)
        {
            int userId = GetUserId();
            var response = new ServiceResponse<GetWeaponDTO>();
            var newWeapon = this.mapper.Map<Weapon>(addWeaponDTO);

            var userCharacters = await this.dataContext.Characters
                .Where(c => c.User!.Id == userId)
                .ToListAsync();

            if (userCharacters == null)
            {
                response.Success = false;
                response.Message = "User doesnt have any characters";
                response.StatusCode = 422;
                return response;
            }

            var foundCharacter = userCharacters.FirstOrDefault(
                c => c.Id == addWeaponDTO.CharacterId
            );

            if (foundCharacter is null)
            {
                response.Success = false;
                response.Message = $"Character Id {addWeaponDTO.CharacterId} for user {userId} not found";
                response.StatusCode = 404;
                return response;
            }

            foundCharacter.Weapon = newWeapon;
            try
            {
                await dataContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                
                response.Success = false;
                response.StatusCode = 500;
                response.Message = e.Message;
                if (e.InnerException != null)
                {
                    response.Details = e.InnerException.Message;
                }
                return response;
            }

            response.Data = mapper.Map<GetWeaponDTO>(newWeapon);
            response.Message = "added weapon for character";
            return response;
        }
    }
}