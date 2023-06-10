using System;

namespace bookstore.Shared
{
    public class ItemPedido
    {
        public String ItemPedidoId { get; set; }
        public String PedidoId { get; set; }
        public int Cantidad { get; set; }
        public String LibroPedidoId { get; set; }
    }
}
