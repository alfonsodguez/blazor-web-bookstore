using System;
using System.Collections.Generic;

namespace bookstore.Shared
{
    public class RESTMessage
    {
        public String Mensaje { get; set; }
        public List<String> Errores { get; set; }
        public String Token { get; set; }
        public Cliente DatosCliente { get; set; }
        public Object Datos { get; set; }
    }
}
