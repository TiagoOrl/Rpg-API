using System.Security.Claims;
using AutoMapper;
using first_api.models;
using first_api.DTO.character;

namespace first_api.service.character_service
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper mapper; 
        private readonly DataContext dataContext;
        private readonly IHttpContextAccessor httpAcessor;

        public CharacterService(IMapper mapper, DataContext dataContext, IHttpContextAccessor httpAcessor)
        {
            this.httpAcessor = httpAcessor;
            this.mapper = mapper;
            this.dataContext = dataContext;
        }

        // gets the user id from the jwt passed in the http request header
        private int GetUserId() => int.Parse(httpAcessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto addCharacterDto)
        {
            int userId = GetUserId();
            var response = new ServiceResponse<List<GetCharacterDto>>();
            var newCharacter = this.mapper.Map<Character>(addCharacterDto);

            newCharacter.User = await dataContext.Users.FirstOrDefaultAsync(
                u => u.Id == userId
            );

            dataContext.Characters.Add(newCharacter);
            await dataContext.SaveChangesAsync();
            response.Data = 
                await dataContext.Characters
                    .Where(c => c.User!.Id == userId)
                    .Select(c => mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            int userId = GetUserId();

            var response = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await dataContext.Characters
                .Include(c => c.Weapon) // include the nested Entity
                .Include(c => c.Skills) // include the nested Entity
                .Where(c => c.User!.Id == userId)
                .ToListAsync();
            response.Data = dbCharacters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            int userId = GetUserId();

            var response = new ServiceResponse<GetCharacterDto>();
            var foundCharacter =  await dataContext.Characters
                .Include(c => c.Weapon) // include the nested Entity
                .Include(c => c.Skills) // include the nested Entity
                .FirstOrDefaultAsync(
                    c => c.Id == id && c.User!.Id == userId
                );

            if (foundCharacter is null) {
                response.Success = false;
                response.Message = $"character of id '{id}' for userId '{userId}' not found";
                return response;
            }

            response.Data = this.mapper.Map<GetCharacterDto>(foundCharacter);
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateDto)
        {
            int userId = GetUserId();

            var response = new ServiceResponse<GetCharacterDto>();
            var foundCharacter = dataContext.Characters.FirstOrDefault(
                c => c.Id == updateDto.Id && c.User!.Id == userId
            );

            if (foundCharacter == null)
            {
                response.Success = false;
                response.Message = $"character of id '{updateDto.Id}' for userId '{userId}' not found";
                return response;
            }

            // copy data from the requestDto to the found character
            var updatedCharacter = mapper.Map(updateDto, foundCharacter); 

            //equivatent operation to the map above
            
            // foundCharacter.Name = updateDto.Name;
            // foundCharacter.Class = updateDto.Class;
            // foundCharacter.Strength = updateDto.Strength;
            // foundCharacter.Intelligence = updateDto.Intelligence;
            // foundCharacter.HitPoints = updateDto.HitPoints;
            // foundCharacter.Defense = updateDto.Defense;

            await dataContext.SaveChangesAsync();
            response.Data = mapper.Map<GetCharacterDto>(foundCharacter);
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> DeleteCharacter(int id)
        {
            int userId = GetUserId();
            var responseBody = new ServiceResponse<GetCharacterDto>();
            var foundCharacter = await dataContext.Characters.FirstOrDefaultAsync(
                c => c.Id == id && c.User!.Id == userId
            );

            if (foundCharacter is null) {
                responseBody.Success = false;
                responseBody.Message = $"character of id '{id}' not found";
                return responseBody;
            }

            dataContext.Characters.Remove(foundCharacter);
            await dataContext.SaveChangesAsync();
            responseBody.Data = mapper.Map<GetCharacterDto>(foundCharacter);

            return responseBody;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharSkillDto inputDto)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            var userId = GetUserId();
            try
            {
                var foundCharacter = await dataContext.Characters
                    .Include(i => i.User)
                    .Include(i => i.Skills)
                    .FirstOrDefaultAsync(
                        c => c.Id == inputDto.CharacterId && c.User!.Id == userId
                    );

                if (foundCharacter == null)
                {
                    response.Success = false;
                    response.StatusCode = 404;
                    response.Message = $"character id {inputDto.CharacterId} with user id {userId} not found";
                    return response;
                }

                var skill = await dataContext.Skills.FirstOrDefaultAsync(
                    s => s.Id == inputDto.SkillId
                );

                if (skill == null)
                {
                    response.Success = false;
                    response.StatusCode = 404;
                    response.Message = $"skill id {inputDto.SkillId} not found";
                    return response;
                }

                foundCharacter.Skills!.Add(skill);
                await dataContext.SaveChangesAsync();

                response.Data = mapper.Map<GetCharacterDto>(foundCharacter);
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}