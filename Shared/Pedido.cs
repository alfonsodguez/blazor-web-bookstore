using System;
using System.Collections.Generic;

namespace bookstore.Shared
{
    public class Pedido
    {
        #nullable enable
        public String PedidoId { get; set; }
        public String? ClienteId { get; set; }
        public String Estado { get; set; }
        public DateTime Fecha { get; set; }
        public Decimal GastosEnvio { get; set; }
        public Decimal SubTotal { get; set; }
        public Decimal Total { get; set; }
        #nullable disable

        public List<String> ElementosPedido { get; set; }

        public Pedido()
        {
            this.PedidoId = System.Guid.NewGuid().ToString();
            this.ElementosPedido = new List<String>();
        }
    }
}
