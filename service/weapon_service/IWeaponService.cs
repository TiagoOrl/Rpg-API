using first_api.models;
using first_api.DTO.weapon;

namespace first_api.service.weapon_service
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetWeaponDTO>> AddWeapon(AddWeaponDTO addWeaponDTO);
    }
}