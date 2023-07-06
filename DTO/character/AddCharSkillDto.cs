using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace first_api.DTO.character
{
    public class AddCharSkillDto
    {
        public int CharacterId { get; set; }
        public int SkillId { get; set; }
    }
}