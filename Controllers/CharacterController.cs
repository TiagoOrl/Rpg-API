using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using first_api.models;
using Microsoft.AspNetCore.Mvc;

namespace first_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private static List<Character> characters = new List<Character> {
            new Character {Id = 1, Name = "Jeff", Class = RpgClass.Cleric },
            new Character {Id = 2, Name = "Ana", Class = RpgClass.Knight },
            new Character {Id = 3, Name = "James", Class = RpgClass.Mage },
            new Character {Id = 4, Name = "Clara", Class = RpgClass.Mage }
        };

        [HttpGet("get-all")]
        public ActionResult<List<Character>> GetAllChars() 
        {
            return Ok(characters);
        }

        [HttpGet("get-single/{Id}")]
        public ActionResult<Character> GetOne(int Id) 
        {
            return Ok(characters.FirstOrDefault(c => {
                return c.Id == Id;
            }));
        }

        [HttpPost]
        public ActionResult<List<Character>> AddCharacter(Character character) 
        {
            characters.Add(character);
            return Ok(characters);
        }
    }
}