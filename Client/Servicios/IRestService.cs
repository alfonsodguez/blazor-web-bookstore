using bookstore.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookstore.Client.Servicios
{
    public interface IRestService
    {
        Task<List<Provincia>> DevolverProvincias();
        Task<List<Municipio>> DevolverMunicipios(int codpro);
        Task<RESTMessage> RegistrarCliente(Cliente cliente);
        Task<RESTMessage> LoginCliente(Cliente.Credenciales creds);
        Task<List<Libro>> DevolverLibros(int idCategoria);
        Task<Libro> DevolverLibro(string libroId);
        Task<List<Materia>> DevolverMaterias(int idMateria);
    }
}
