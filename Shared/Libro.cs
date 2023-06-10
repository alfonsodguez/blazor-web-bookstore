using System;

namespace bookstore.Shared
{
    public class Libro
    {
        public String LibroId { get; set; }
        public String Titulo { get; set; }
        public String Editorial { get; set; }
        public String Autor { get; set; }
        public String ISBN10 { get; set; }
        public String ISBN13 { get; set; }
        public String Descripcion { get; set; }
        public int NumeroPaginas { get; set; }
        public Decimal Precio { get; set; }
        public String FicheroImagen { get; set; }
        public int IdMateria { get; set; }
    }
}
