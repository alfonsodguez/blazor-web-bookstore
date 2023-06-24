using bookstore.Server.Models;
using bookstore.Server.Models.Interfaces;
using bookstore.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace bookstore.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RESTClienteServiceController : ControllerBase
    {
        private UserManager<ClienteIdentity> _userManager;
        private SignInManager<ClienteIdentity> _siginManager;
        private AplicacionDBContext _db;
        private IClienteEmail _clienteEmail;
        private IConfiguration _ficheroConfig;

        public RESTClienteServiceController(UserManager<ClienteIdentity> userMangServiceDI, SignInManager<ClienteIdentity> signInMangServiceDI, AplicacionDBContext dbContextDI, IClienteEmail mailjetClienteDI, IConfiguration accesoAppsettingsDI)
        {
            this._userManager = userMangServiceDI;
            this._siginManager = signInMangServiceDI;
            this._db = dbContextDI;
            this._clienteEmail = mailjetClienteDI;
            this._ficheroConfig = accesoAppsettingsDI;
        }


        [HttpPost]
        public async Task<RESTMessage> Login([FromBody] Cliente.Credenciales credenciales)
        {
            try
            {
                string email = credenciales.Email;
                string password = credenciales.Password;

                ClienteIdentity cliente = await this._userManager.FindByEmailAsync(email);
                if (!cliente.EmailConfirmed)
                {
                    throw new Exception("Email sin confirmar");
                }

                Microsoft.AspNetCore.Identity.SignInResult checkLogin = await this._siginManager.PasswordSignInAsync(cliente, password, true, false);
                if (checkLogin.Succeeded)
                {
                    String clienteId = cliente.Id;
                    String userName = cliente.UserName;

                    Cliente.Credenciales publicCredenciales = new Cliente.Credenciales { Email = email, Login = userName};
                    
                    List<Direccion> direcciones = this._db.Direcciones
                        .Where((Direccion direccion) => direccion.ClienteId == clienteId)
                        .ToList<Direccion>();

                    Pedido newPedido = new Pedido
                    {
                        ClienteId = clienteId,
                        PedidoId = Guid.NewGuid().ToString(),
                        Estado = "pendiente",
                        Fecha = DateTime.Now,
                        GastosEnvio = 2,
                        SubTotal= 0,
                        Total = 0
                        ElementosPedido = new List<String>(),
                    };
s
                    Cliente infoCliente = new Cliente
                    {
                        ClienteId = clienteId,
                        NIF = cliente.NIF,
                        Nombre = cliente.Nombre,
                        Apellidos = cliente.Apellidos,
                        Telefono = cliente.PhoneNumber,
                        CredencialesCliente = publicCredenciales,
                        ImagenAvatar = cliente.ImagenAvatar ?? "",
                        Descripcion = "",
                        ListaDirecciones = direcciones,
                        PedidoActual = newPedido,
                        HistoricoPedidos = new List<Pedido>()
                    };

                    return new RESTMessage
                    {
                        Mensaje = "Login success",
                        Errores = null,
                        DatosCliente = infoCliente,
                        Datos = null,
                        Token = this.GenerarJWTsesion(cliente, credenciales)
                    };
                }
                throw new Exception("Password incorrecta");
            }
            catch (Exception ex)
            {
                return new RESTMessage
                {
                    Mensaje = "Credenciales incorrectas",
                    Errores = new List<String> { "Email invalido", "Emsail sin confirmar" },
                    DatosCliente = null,
                    Token = null,
                    Datos = null
                };
            }
        }

        [HttpPost]
        public async Task<RESTMessage> Registro([FromBody] Cliente datosCliente)
        {
            string password = datosCliente.CredencialesCliente.Password;
            string email = datosCliente.CredencialesCliente.Email;

            ClienteIdentity nuevoCliente = new ClienteIdentity
            {
                Nombre = datosCliente.Nombre,
                Apellidos = datosCliente.Apellidos,
                NIF = datosCliente.NIF,
                Email = email,
                UserName = datosCliente.CredencialesCliente.Login,
                PhoneNumber = datosCliente.Telefono
            };

            IdentityResult registroCliente = await this._userManager.CreateAsync(nuevoCliente, password);
            if (registroCliente.Succeeded)
            {
                ClienteIdentity cliente = await this._userManager.FindByEmailAsync(email);

                Direccion direccion = new Direccion
                {
                    ClienteId = cliente.Id
                    DireccionId = Guid.NewGuid().ToString(),
                    Calle = datosCliente.ListaDirecciones[0].Calle,
                    CP = datosCliente.ListaDirecciones[0].CP,
                    CodProvincia = datosCliente.ListaDirecciones[0].CodProvincia,
                    CodMunicipio = datosCliente.ListaDirecciones[0].CodMunicipio,
                    EsPrincipal = datosCliente.ListaDirecciones[0].EsPrincipal,
                    Tipo = datosCliente.ListaDirecciones[0].Tipo,
                };

                this._db.Direcciones.Add(direccion);
                await this._db.SaveChangesAsync();
                await this.EnviarEmailRegistro(cliente);

                return new RESTMessage
                {
                    Mensaje = "Registro OK, email de confirmacion enviado",
                    Errores = null,
                    DatosCliente = null,
                    Token = null,
                    Datos = null
                };
            }

            return new RESTMessage
            {
                Mensaje = "Fallo en el registro",
                Errores = new List<String> { "Fallo en el registro, Fallo en servidor al almacenar los datos del cliente" },
                DatosCliente = null,
                Token = null,
                Datos = null
            };
        }

        [HttpGet(Name = "activarCuenta")]
        public async Task<IActionResult> ConfirmarRegistro([FromQuery] String idCliente, [FromQuery] String token)
        {
            ClienteIdentity cliente = await this._userManager.FindByIdAsync(idCliente);
            IdentityResult validarToken = await this._userManager.ConfirmEmailAsync(cliente, token);

            if (validarToken.Succeeded)
            {
                string urlLogin = "https://localhost:44351/Cliente/Login";
                return Redirect(urlLogin);
            }

            return Ok(new RESTMessage
            {
                Mensaje = "Confirmacion Email fallida",
                Errores = new List<String> { "Activación de email mediante Identity fallida" },
                Datos = null,
                DatosCliente = null,
                Token = null
            });
        }


        private String GenerarJWTsesion(ClienteIdentity cliente, Cliente.Credenciales credenciales)
        {
            SecurityKey claveFirma = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(this._ficheroConfig["JWT:clave"]));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: this._ficheroConfig["JWT:issuer"],
                audience: null,
                claims: new List<Claim>
                {
                    new Claim("Nombre", cliente.Nombre),
                    new Claim("Apellidos", cliente.Apellidos),
                    new Claim("NIF", cliente.NIF),
                    new Claim("ClienteId", cliente.Id),
                    new Claim("Email", credenciales.Email)
                },
                expires: DateTime.Now.AddHours(2),
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(claveFirma, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task EnviarEmailRegistro(ClienteIdentity cliente)
        {
            string token = await this._userManager.GenerateEmailConfirmationTokenAsync(cliente);
            string ruta = "activarCuenta";
            string protocolo = "https";
            string host = "localhost:44351";
            string metadata = new { idcliente = cliente.Id, token = token };
            string urlEmail = Url.RouteUrl(ruta, metadata, protocolo, host);
            string mensaje = $@"
                        <h3><strong>Se ha registrado correctamente en Agapea.com</strong></h3>
                        <br>Haz click en <a href='{urlEmail}'>{urlEmail}</a> para activar tu cuenta";
            string destinatario = cliente.Email;
            string asunto = "Confirmacion de registro en Agapea.com";

            this._clienteEmail.EnviarEmail(destinatario, asunto, mensaje, null);
        }
    }
}
