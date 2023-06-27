using System.Security.Cryptography;
using first_api.models;
using first_api.DTO.user;

namespace first_api.service.user_service
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext dataContext;
        public AuthRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public Task<ServiceResponse<string>> Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<int>> Register(UserRegisterDto userDto, string password)
        {
            var response = new ServiceResponse<int>();
            var newUser = new User();

            if (await UserExists(userDto.Username))
            {
                response.Success = false;
                response.Message = $"user with username {userDto.Username} already exists";
                return response;
            }

            CreateHash(password, out byte[] passwordHash, out byte[] salt);
            newUser.PasswordHash = passwordHash;
            newUser.Salt = salt;
            newUser.Username = userDto.Username;

            dataContext.Users.Add(newUser);
            await dataContext.SaveChangesAsync();

            response.Data = newUser.Id;
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            var foundUser = await dataContext.Users.FirstOrDefaultAsync(
                u => u.Username.ToLower() == username.ToLower()
            );

            if (foundUser != null)
            {
                return true;
            }
            return false;
        }

        private void CreateHash(string password, out byte[] hashOutput, out byte[] passwordSalt)
        {
            var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            hashOutput = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}