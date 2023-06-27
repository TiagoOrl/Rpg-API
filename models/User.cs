using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace first_api.models
{
    public class User
    {
        public int Id { get; set; }
        public String Username { get; set; } = string.Empty;
        public byte[]? PasswordHash { get; set; } = null;
        public byte[]? Salt { get; set; } = null;
        public List<Character>? Characters { get; set; } = null;
    }
}