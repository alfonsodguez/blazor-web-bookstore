using bookstore.Server.Models.Interfaces;
using bookstore.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookstore.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RESTProvsMunisController : ControllerBase
    {
        private IDBAccess _accesoDB;

        public RESTProvsMunisController(IDBAccess servicioDBAccessDI)
        {
            this._accesoDB = servicioDBAccessDI;
        }


        [HttpGet]
        public async Task<RESTMessage> DevolverMunicipios([FromQuery] String codProvincia)
        {
            List<Municipio> municipios = await this._accesoDB.DevolverMunicipios(System.Convert.ToInt16(codProvincia));

            if (municipios != null)
            {
                return new RESTMessage
                {
                    DatosCliente = null,
                    Errores = null,
                    Token = null,
                    Mensaje = "Muncipios recuperados OK",
                    Datos = municipios
                };
            }

            return new RESTMessage
            {
                DatosCliente = null,
                Errores = new List<string> { "Error al recuperar los muncipios correspondientes al código de provincia: " + codProvincia },
                Token = null,
                Mensaje = "Municipios recuperados KO",
                Datos = null
            };
        }

        [HttpGet]
        public async Task<RESTMessage> DevolverProvincias()
        {
            List<Provincia> provincias = await this._accesoDB.DevolverProvincias();

            if (provincias != null)
            {
                return new RESTMessage
                {
                    DatosCliente = null,
                    Errores = null,
                    Token = null,
                    Mensaje = "Provincias recuperadas OK",
                    Datos = provincias
                };
            }

            return new RESTMessage
            {
                DatosCliente = null,
                Errores = new List<string> { "Error al recuperar las provincias" },
                Token = null,
                Mensaje = "Provincias recuperadas KO",
                Datos = null
            };
        }
    }
}
