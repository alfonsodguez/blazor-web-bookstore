using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using bookstore.Server.Models.Interfaces;
using bookstore.Shared;

namespace bookstore.Server.Models
{
    public class SqlServerDBAccess : IDBAccess
    {
        private readonly IConfiguration _accessAppSettings;
        private readonly String _cadenaConexionDB;

        public ServicioDBAccess(IConfiguration accesoConfigInyect)
        {
            this._accessAppSettins = accesoConfigInyect;
            this._cadenaConexionDB = this._accessAppSettings.GetConnectionString("AgapeaDBConnectionString");
        }


        public async Task<List<Municipio>> DevolverMunicipios(int codpro)
        {
            try
            {
                SqlConnection conexionDB = new SqlConnection(this._cadenaConexionDB);
                await conexionDB.OpenAsync();

                SqlCommand selectMunicipios = new SqlCommand("SELECT * FROM dbo.Municipios WHERE CodProvincia=@prov", conexionDB);
                selectMunicipios.Parameters.AddWithValue("@prov", codpro);
                SqlDataReader cursorMunicipios = await selectMunicipios.ExecuteReaderAsync();

                return cursorMunicipios
                    .Cast<IDataRecord>()
                    .Select((fila) => new Municipio
                    {
                        CodProvincia = System.Convert.ToInt16(fila["CodProvincia"]),
                        CodMunicipio = System.Convert.ToInt16(fila["CodMunicipio"]),
                        Nombre = fila["Nombre"].ToString()
                    })
                    .ToList<Municipio>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Provincia>> DevolverProvincias()
        {
            try
            {
                SqlConnection conexionDB = new SqlConnection(this._cadenaConexionDB);
                await conexionDB.OpenAsync();

                SqlCommand selectProvincias = new SqlCommand("SELECT * FROM dbo.Provincias", conexionDB);
                SqlDataReader cursorProvincias = await selectProvincias.ExecuteReaderAsync();

                return cursorProvincias
                    .Cast<IDataRecord>()
                    .Select((fila) => new Provincia 
                    {  
                        CodProvincia = System.Convert.ToInt32(fila["CodProvincia"]),
                        Nombre = fila["Nombre"].ToString()
                    })
                    .ToList<Provincia>();
            }
            catch (Exception ex)
            {
                return null;
            }    
        }
    }
}
