using bookstore.Shared;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookstore.Client.Servicios
{
    public class StorageService : IStorageService
    {
        private IJSRuntime _js;
        public DotNetObjectReference<StorageService> StorageSrvReference;
        public event EventHandler<Cliente> ClienteRecuperadoIndexedDBEvent;
        public event EventHandler<List<Tuple<Libro, int>>> ItemsRecuperadosIndexedDBEvent;


        public StorageService(IJSRuntime javascriptDI)
        {
            this.StorageSrvReference = DotNetObjectReference.Create(this);
            this._js = javascriptDI;
        }

        public async Task EliminarIndexedDB()
        {
            await this._js.InvokeVoidAsync("manageIndexedDB.borrarIndexedDB");
        }

        public async Task InsertarClienteJWTEnStorage(Cliente cliente, string jwt)
        {
            await this._js.InvokeVoidAsync("manageIndexedDB.almacenarClienteJWT", cliente, jwt);
        }

        public async Task<bool> EstarClienteLogueado()
        {
            return await this._js.InvokeAsync<bool>("manageIndexedDB.checkIsLogged");
        }

        public async Task InsertarItemtsPedidoEnStorage(List<Tuple<Libro, int>> listaItems)
        {
            await this._js.InvokeVoidAsync("manageIndexedDB.almacenarItemsPedido", listaItems);
        }

        public async Task DevolverClienteDelStorage()
        {
            await this._js.InvokeAsync<Cliente>("manageIndexedDB.devolverCliente", this.StorageSrvReference);
        }

        [JSInvokable("BlazorDBCallback")]
        public void CalledFromJS(Cliente cliente)
        {
            this.ClienteRecuperadoIndexedDBEvent.Invoke(this, cliente);
        }

        public async Task DevolverItemsPedidoDelStorage()
        {
            await this._js.InvokeAsync<Cliente>("manageIndexedDB.devolverItemsPedido", this.StorageSrvReference);
        }

        [JSInvokable("BlazorDBCallbackItemsPedido")]
        public void CalledFromJSItems(List<Tuple<Libro, int>> lista)
        {
            this.ItemsRecuperadosIndexedDBEvent.Invoke(this, lista);
        }

    }
}
