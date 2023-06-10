using bookstore.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookstore.Server.Models.Interfaces
{
    public interface IDBAccess
    {
        Task<List<Provincia>> DevolverProvincias();
        Task<List<Municipio>> DevolverMunicipios(int codProvincia);
    }
}
