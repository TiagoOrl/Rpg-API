

namespace first_api.models
{
    public class Skill
    {
        public int Id {get; set;}
        public string Name {get; set;} = string.Empty;
        public int DamageVal {get; set;}
        public List<Character>? Characters {get; set;}
    }
}