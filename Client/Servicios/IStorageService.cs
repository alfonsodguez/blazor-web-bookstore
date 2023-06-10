using bookstore.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookstore.Client.Servicios
{
    public interface IStorageService
    {
        public event EventHandler<Cliente> ClienteRecuperadoIndexedDBEvent;
        public event EventHandler<List<Tuple<Libro, int>>> ItemsRecuperadoIndexedDBEvent;

        Task DeleteIndexedDB();
        Task<bool> EstaClienteLogueado();
        Task DevuelveClienteDelStorage();
        Task DevuelveItemsPedidoDelStorage();
        Task InsertaClienteJWTEnStorage(Cliente cliente, String jwt);
        Task InsertaItemtsPedidoEnStorage(List<Tuple<Libro, int>> listaItems);
    }
}
