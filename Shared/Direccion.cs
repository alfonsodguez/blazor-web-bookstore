using System;

namespace bookstore.Shared
{
    public class Direccion
    {
        public String DireccionId { get; set; }
        public String ClienteId { get; set; }
        public String Calle { get; set; }
        public int CP { get; set; }
        public int CodPro { get; set; }
        public int CodMun { get; set; }
        public bool EsPrincipal { get; set; } = false;
        public String TipoDireccion { get; set; } = "direccion hogar";
    }
}
