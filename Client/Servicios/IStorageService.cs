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

        Task EliminarIndexedDB();
        Task<bool> EstarClienteLogueado();
        Task DevolverClienteDelStorage();
        Task DevolverItemsPedidoDelStorage();
        Task InsertarClienteJWTEnStorage(Cliente cliente, String jwt);
        Task InsertarItemtsPedidoEnStorage(List<Tuple<Libro, int>> listaItems);
    }
}
