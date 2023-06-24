using bookstore.Server.Models.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace bookstore.Server.Models
{
    public class ClienteCorreoMAILJET : IClienteEmail
    {
        private IOptions<ConfigMailjet> _configMailjet;

        public ClienteCorreoMAILJET(IOptions<ConfigMailjet> configServerMailject)
        {
            this._configMailjet = configSeverMailject;
        }


        #nullable enable
        public void EnviarEmail(string destinatario, string asunto, string cuerpo, string? nombreAdjunto)
        {
            String ServerName = this._configMailjet.Value.ServerName;
            String ApiKey = this._configMailjet.Value.APIKey;
            String SecretKey = this._configMailjet.Value.SecretKey;

            SmtpClient clienteSMTP = new SmtpClient();
            clienteSMTP.Host = ServerName;
            clienteSMTP.Credentials = new NetworkCredential(ApiKey, SecretKey);

            String remitente = "admin@agapea.com";
            MailMessage mensaje = new MailMessage(remitente, destinatario, asunto, cuerpo);

            if (!String.IsNullOrEmpty(nombreAdjunto))
            {
                FileStream fileContent = new FileStream(nombreAdjunto, FileMode.Open, FileAccess.Read);
                mensaje.Attachments.Add(new Attachment(fileContent, nombreAdjunto, "application/pdf"));
            }
            clienteSMTP.SendAsync(mensaje, null);
        }
    }
}
