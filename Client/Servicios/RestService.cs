using bookstore.Shared;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace bookstore.Client.Servicios
{
    public class RestService : IRestService
    {
        private HttpClient _http;
        private IJSRuntime _js;

        public RestService(HttpClient servicioHttpDI, IJSRuntime servicioJSRuntimeDI)
        {
            this._http = servicioHttpDI;
            this._js = servicioJSRuntimeDI;
        }

        public async Task<List<Municipio>> DevolverMunicipios(int codProvincia)
        {
            String urlMunicipios = "api/RESTProvsMunis/DevolverMunicipios?codProvincia=" + codProvincia;
            HttpResponseMessage respuesta = await this._http.GetAsync(urlMunicipios);
            RESTMessage resultado = JsonSerializer.Deserialize<RESTMessage>(await respuesta.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNamingPolicy = null });

            return JsonSerializer.Deserialize<List<Municipio>>(((JsonElement)resultado.Datos).GetRawText());
        }

        public async Task<List<Provincia>> DevolverProvincias()
        {
            String urlProvincias = "api/RESTProvsMunis/DevolverProvincias";
            HttpResponseMessage respuesta = await this._http.GetAsync(urlProvincias);
            String contenidoRespuesta = await respuesta.Content.ReadAsStringAsync();
            RESTMessage resultado = JsonSerializer.Deserialize<RESTMessage>(contenidoRespuesta, new JsonSerializerOptions { PropertyNamingPolicy = null });

            return JsonSerializer.Deserialize<List<Provincia>>(((JsonElement)resultado.Datos).GetRawText());
        }

        public async Task<RESTMessage> LoginCliente(Cliente.Credenciales credenciales)
        {
            String urlLogin = "api/RESTClienteService/Login";
            HttpResponseMessage respuesta = await this._http.PostAsJsonAsync<Cliente.Credenciales>(urlLogin, credenciales);
            RESTMessage resultado = JsonSerializer.Deserialize<RESTMessage>(await respuesta.Content.ReadAsStringAsync());

            return resultado;
        }

        public async Task<RESTMessage> RegistrarCliente(Cliente cliente)
        {
            String urlRegistro = "api/RESTClienteService/Registro";
            HttpResponseMessage respuesta = await this._http.PostAsJsonAsync<Cliente>(urlRegistro, cliente);

            return await respuesta.Content.ReadFromJsonAsync<RESTMessage>();
        }


        public async Task<List<Materia>> DevolverMaterias(int idMateria)
        {
            String urlMaterias = "api/RESTTiendaService/DevolverMaterias?idMateria=" + idMateria;
            HttpResponseMessage respuesta = await this._http.GetAsync(urlMaterias);
            RESTMessage resultado = JsonSerializer.Deserialize<RESTMessage>(await respuesta.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNamingPolicy = null });

            return JsonSerializer.Deserialize<List<Materia>>(((JsonElement)resultado.Datos).GetRawText());
        }

        public async Task<List<Libro>> DevolverLibros(int idCategoria)
        {
            HttpResponseMessage respuesta = await this._http.GetAsync("api/RESTTiendaService/DevolverLibros?idCategoria=" + idCategoria);
            RESTMessage resultado = JsonSerializer.Deserialize<RESTMessage>(await respuesta.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNamingPolicy = null });

            return JsonSerializer.Deserialize<List<Libro>>(((JsonElement)resultado.Datos).GetRawText());
        }

        public async Task<Libro> DevolverLibro(string idLibro)
        {
            String urlLibros = "api/RESTTiendaService/DevolverLibro?idLibro=" + idLibro;
            HttpResponseMessage respuesta = await this._http.GetAsync(urlLibros);
            RESTMessage resultado = JsonSerializer.Deserialize<RESTMessage>(await respuesta.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNamingPolicy = null });

            return JsonSerializer.Deserialize<Libro>(((JsonElement)resultado.Datos).GetRawText());
        }
    }
}
