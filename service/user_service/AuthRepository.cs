using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using first_api.models;
using first_api.DTO.user;

namespace first_api.service.user_service
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext dataContext;
        private readonly IConfiguration configuration;
        public AuthRepository(DataContext dataContext, IConfiguration configuration)
        {
            this.dataContext = dataContext;
            this.configuration = configuration;
        }
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var response = new ServiceResponse<string>();
            var foundUser = await dataContext.Users.FirstOrDefaultAsync(
                u => u.Username.ToLower() == username.ToLower()
            );

            if (foundUser == null)
            {
                response.Success = false;
                response.Message = $"username or password invalid";
                response.StatusCode = 401;
                return response;
            }

            if (!CheckPassword(password, foundUser.PasswordHash, foundUser.Salt))
            {
                response.Success = false;
                response.Message = "username or password invalid";
                response.StatusCode = 401;
                return response;
            }
            response.Data = CreateToken(foundUser);
            return response;
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

        private bool CheckPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            HMACSHA512 hmac = new HMACSHA512(passwordSalt);
            // generate hash from the password with the salt
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));


            // checks if the computed hash is equal to the hash present in the user database.
            return computedHash.SequenceEqual(passwordHash);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),

            };

            var appToken = configuration.GetSection("AppSettings:Token").Value;
            if (appToken is null)
                throw new Exception("token de app não encontrado");

            SymmetricSecurityKey Appkey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(appToken)
            );

            SigningCredentials signingCredentials = new SigningCredentials(Appkey, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = signingCredentials
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}