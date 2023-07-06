using first_api.models;
using first_api.DTO.weapon;
using first_api.DTO.skill;

namespace first_api.DTO.character
{
    public class GetCharacterDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; }
        public GetWeaponDTO? Weapon {get; set;}
        public List<GetSkillDto>? Skills { get; set; }
    }
}