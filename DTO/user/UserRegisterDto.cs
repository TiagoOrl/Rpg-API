using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace first_api.DTO.user
{
    public class UserRegisterDto
    {
        public string? Password { get; set; }
        public string? Username { get; set; }
    }
}