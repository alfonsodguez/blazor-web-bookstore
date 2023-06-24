using System;

namespace bookstore.Shared
{
    public class Direccion
    {
        public String DireccionId { get; set; }
        public String ClienteId { get; set; }
        public String Calle { get; set; }
        public String Tipo { get; set; } = "Avd.";
        public int CP { get; set; }
        public int CodProvincia { get; set; }
        public int CodMunicipio { get; set; }
        public bool EsPrincipal { get; set; } = false;
    }
}
