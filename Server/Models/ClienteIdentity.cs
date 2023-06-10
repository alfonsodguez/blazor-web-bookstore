using Microsoft.AspNetCore.Identity;
using System;

namespace bookstore.Server.Models
{
    public class ClienteIdentity : IdentityUser
    {
        public String Nombre { get; set; }
        public String Apellidos { get; set; }
        public String NIF { get; set; }
        public String ImagenAvatar { get; set; }
    }
}
