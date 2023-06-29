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

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto addCharacterDto)
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();
            var newCharacter = this.mapper.Map<Character>(addCharacterDto);

            dataContext.Characters.Add(newCharacter);
            await dataContext.SaveChangesAsync();
            response.Data = 
                await dataContext.Characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters(int userId)
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await dataContext.Characters
                .Where(c => c.User!.Id == userId)
                .ToListAsync();
            response.Data = dbCharacters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            var foundCharacter =  await dataContext.Characters.FirstOrDefaultAsync(
                c => c.Id == id
            );

            if (foundCharacter is null) {
                response.Success = false;
                response.Message = $"character of id '{id}' not found";
                return response;
            }

            response.Data = this.mapper.Map<GetCharacterDto>(foundCharacter);
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateDto)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            var foundCharacter = dataContext.Characters.FirstOrDefault(c => c.Id == updateDto.Id);

            if (foundCharacter == null)
            {
                response.Success = false;
                response.Message = $"character of id '{updateDto.Id}' not found";
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
            var responseBody = new ServiceResponse<GetCharacterDto>();
            var foundCharacter = await dataContext.Characters.FirstOrDefaultAsync(
                c => c.Id == id
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
    }
}