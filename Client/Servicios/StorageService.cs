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
        public event EventHandler<List<Tuple<Libro, int>>> ItemsRecuperadoIndexedDBEvent;


        public StorageService(IJSRuntime javascriptDI)
        {
            this.StorageSrvReference = DotNetObjectReference.Create(this);
            this._js = javascriptDI;
        }


        public async Task DevuelveClienteDelStorage()
        {
            await this._js.InvokeAsync<Cliente>("manageIndexedDB.devuelveCliente", this.StorageSrvReference);
        }

        [JSInvokable("BlazorDBCallback")]
        public void CalledFromJS(Cliente cliente)
        {
            this.ClienteRecuperadoIndexedDBEvent.Invoke(this, cliente);
        }

        public async Task DevuelveItemsPedidoDelStorage()
        {
            await this._js.InvokeAsync<Cliente>("manageIndexedDB.devuelveItemsPedido", this.StorageSrvReference);
        }

        [JSInvokable("BlazorDBCallbackItemsPedido")]
        public void CalledFromJSItems(List<Tuple<Libro, int>> lista)
        {
            this.ItemsRecuperadoIndexedDBEvent.Invoke(this, lista);
        }

        public async Task DeleteIndexedDB()
        {
            await this._js.InvokeVoidAsync("manageIndexedDB.borrarDB");
        }

        public async Task InsertaClienteJWTEnStorage(Cliente cliente, string jwt)
        {
            await this._js.InvokeVoidAsync("manageIndexedDB.almacenaClienteJWT", cliente, jwt);
        }

        public async Task<bool> EstaClienteLogueado()
        {
            return await this._js.InvokeAsync<bool>("manageIndexedDB.checkIsLogged");
        }

        public async Task InsertaItemtsPedidoEnStorage(List<Tuple<Libro, int>> listaItems)
        {
            await this._js.InvokeVoidAsync("manageIndexedDB.almacenaItemsPedido", listaItems);
        }
    }
}
