using System;

namespace bookstore.Server.Models.Interfaces
{
    public interface IClienteEmail
    {
        #nullable enable
        void EnviarEmail(String destinatario, String asunto, String cuerpo, String? nombreAdjunto);
    }
}
