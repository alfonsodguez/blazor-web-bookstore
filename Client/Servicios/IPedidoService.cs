using bookstore.Shared;
using System.Collections.Generic;

namespace bookstore.Client.Servicios
{
    public interface IPedidoService
    {
        public void AñadirLibroAlCarrito(Libro libro, int cantidad = 1);
        public List<ItemPedido> DevolverItemsPedido();
        public void EliminarLibroDelCarrito(Libro libro);
        public void ModificarLibroDelCarrito(Libro libro, int cantidad);
    }
}
