using bookstore.Shared;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace bookstore.Client.Servicios
{
    public class PedidoService : IPedidoService
    {
        private BehaviorSubject<List<ItemPedido>> _itemsPedidoSubject = new BehaviorSubject<List<ItemPedido>>(new List<ItemPedido>());
        private List<ItemPedido> _listaItems;

        public PedidoService()
        {
            this._RecuperaItemsSubject();
        }


        private void _RecuperaItemsSubject()
        {
            this._itemsPedidoSubject.Subscribe((List<ItemPedido> items) => this._listaItems = items);
        }

        public void AñadirLibroAlCarrito(Libro libro, int cantidad = 1)
        {
            try
            {
                ItemPedido itemEnPedido = this._listaItems.Find((ItemPedido item) => item.LibroPedidoId == libro.LibroId);
                itemEnPedido.Cantidad += cantidad;

                this._itemsPedidoSubject.OnNext(this._listaItems);
            }
            catch (NullReferenceException ex)
            {
                this._listaItems.Add(new ItemPedido
                {
                    Cantidad = cantidad,
                    LibroPedidoId = libro.LibroId,
                    ItemPedidoId = Guid.NewGuid().ToString()
                });

                this._itemsPedidoSubject.OnNext(this._listaItems);
            }
        }

        public List<ItemPedido> DevolverItemsPedido()
        {
            return this._listaItems;
        }

        public void ModificarLibroDelCarrito(Libro libro, int cantidad)
        {
            int pos = this._listaItems.FindIndex((ItemPedido item) => item.LibroPedidoId == libro.LibroId);
            this._listaItems[pos].Cantidad = cantidad;

            this._itemsPedidoSubject.OnNext(this._listaItems);
        }

        public void EliminarLibroDelCarrito(Libro libro)
        {
            this._listaItems.Remove(this._listaItems.Find((ItemPedido item) => item.LibroPedidoId == libro.LibroId));

            this._itemsPedidoSubject.OnNext(this._listaItems);
        }
    }
}
