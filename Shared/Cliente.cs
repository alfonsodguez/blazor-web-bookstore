using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bookstore.Shared
{
    public class Cliente
    {
        [Required(ErrorMessage = "* Nombre obligatorio")]
        [MaxLength(50, ErrorMessage = "El nombre no puede exceder de 50 caracteres")]
        public String Nombre { get; set; }

        [Required(ErrorMessage = "* Apellidos obligatorios")]
        [MaxLength(100, ErrorMessage = "Los apellidos no pueden exceder de 100 caracteres")]
        public String Apellidos { get; set; }

        [Required(ErrorMessage = "* NIF obligatorio")]
        [RegularExpression("^[0-9]{8}-?[a-zA-Z]$", ErrorMessage = "Formato de NIF invalido: 12345678-A")]
        public String NIF { get; set; }

        [Required(ErrorMessage = "* Teléfono obligatorio")]
        [RegularExpression("^[0-9]{3} [0-9]{2} [0-9]{2} [0-9]{2}$", ErrorMessage = "Formato de tlfno invalido: 000 11 22 33")]
        public String TelefonoContacto { get; set; }

        public String ClienteId { get; set; }
        public String Descripcion { get; set; } = "";
        public String ImagenAvatar { get; set; } = "";
        public List<Direccion> ListaDirecciones { get; set; }
        public Credenciales CredencialesCliente { get; set; }
        public Pedido PedidoActual { get; set; }
        public List<Pedido> HistoricoPedidos { get; set; }


        public Cliente()
        {
            this.CredencialesCliente = new Credenciales();
            this.ListaDirecciones = new List<Direccion>();
            this.PedidoActual = new Pedido
            {
                FechaPedido = DateTime.Now,
                EstadoPedido = "en curso"
            };
            this.HistoricoPedidos = new List<Pedido>();
        }

        public class Credenciales
        {
            [Required(ErrorMessage = "* Email obligatorio")]
            [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Formato de email invalido")]
            public String Email { get; set; }

            [Required(ErrorMessage = "* Password obligatoria")]
            [MinLength(4, ErrorMessage = "Se requieren al menos 4 caracteres")]
            [MaxLength(20, ErrorMessage = "La Password no debe contener más de 20 caracteres")]
            [RegularExpression("^(?=.*[A-Z].*[A-Z])(?=.*[!@#$&*])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{4,}$", ErrorMessage = "la password debe tener al menos una letra min, letra MAYS, numero y simbolo")]
            public String Password { get; set; }
            #nullable enable
            public String? HashPasword { get; set; }
            #nullable disable
            public String Login { get; set; }
        }
    }
}
