using System;
using System.Collections.Generic;

namespace bookstore.Shared
{
    public class Pedido
    {
        #nullable enable
        public String PedidoId { get; set; }
        public String? ClienteId { get; set; }
        public DateTime FechaPedido { get; set; }
        public String EstadoPedido { get; set; }
        public Decimal SubTotalPedido { get; set; }
        public Decimal GastosEnvio { get; set; }
        public Decimal TotalPedido { get; set; }
        #nullable disable

        public List<String> ElementosPedido { get; set; }

        public Pedido()
        {
            this.PedidoId = System.Guid.NewGuid().ToString();
            this.ElementosPedido = new List<String>();
        }
    }
}
