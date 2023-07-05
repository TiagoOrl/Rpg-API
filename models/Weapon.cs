
namespace first_api.models
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Damage { get; set; }
        public Character? Character { get; set; } = null;
        public int CharacterId { get; set; }

    }
}