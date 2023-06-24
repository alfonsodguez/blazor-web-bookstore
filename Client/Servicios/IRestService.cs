using bookstore.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookstore.Client.Servicios
{
    public interface IRestService
    {
        Task<List<Provincia>> DevolverProvincias();
        Task<List<Municipio>> DevolverMunicipios(int codProvincia);
        Task<RESTMessage> RegistrarCliente(Cliente cliente);
        Task<RESTMessage> LoginCliente(Cliente.Credenciales credenciales);
        Task<List<Libro>> DevolverLibros(int idCategoria);
        Task<Libro> DevolverLibro(string idLibro);
        Task<List<Materia>> DevolverMaterias(int idMateria);
    }
}
