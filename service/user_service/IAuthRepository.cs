using first_api.models;
using first_api.DTO.user;

namespace first_api.service.user_service
{
    public interface IAuthRepository
    {
        public Task<ServiceResponse<int>> Register(UserRegisterDto user, string password);
        public Task<ServiceResponse<string>> Login(string username, string password);
        public Task<bool> UserExists(string username);
    }
}