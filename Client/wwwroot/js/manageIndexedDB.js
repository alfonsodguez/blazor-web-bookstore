window.manageIndexedDB = {
    borrarDB: () => {
        let _reqDelete = window.indexedDB.deleteDatabase('clientes');
        _reqDelete.addEventListener('success', () => console.log('BD borrada con exito'));
        _reqDelete.addEventListener('error', (err) => console.log('error al borrar bd...',err));
    },
    devuelveCliente: (dotnetReference) => {
        var _promiseCli = new Promise(
            (resolve, reject) => {
                let _reqDb = window.indexedDB.open('clientes');
                _reqDb.addEventListener('upgradeneeded', (ev) => {
                    let _db = _reqDb.result;
                    let _store = _db.createObjectStore('infoClientes', { keyPath: 'nif' });
                    _store.createIndex('nif', 'nif');

                    let _store2 = _db.createObjectStore('tokens', { autoIncrement: true });
                    let _store3 = _db.createObjectStore('itemsPedidoActual', { autoIncrement: true });
                }); //cierre upgradeneeded DB
                _reqDb.addEventListener('success', (ev) => {
                    let _bd = _reqDb.result;

                    let _selectReq = _bd.transaction(['infoClientes'], 'readonly').objectStore('infoClientes').getAll();
                    _selectReq.addEventListener('success', (evt2) => {
                        let _datos = _selectReq.result;

                        console.log('datos recuperados...', _datos[0]);
                        if (_datos[0] != undefined && _datos[0] != null) {
                            resolve(_datos[0]);
                        } else {
                            resolve(null);
                        }
                    });
                    _selectReq.addEventListener('error', (error) => {
                        console.log('errores al recuperar sesion cliente..', error);
                        resolve(null);
                    });
                }); //cierre success DB
                _reqDb.addEventListener('error', (errDB) => {
                    console.log('error al abrir bd-clientes en indexedDB....', errDB);
                    resolve(null);
                }); //cierre error DB

            }
        ); //cierre promise

        _promiseCli.then(datos => {
            console.log('invocando desde js a metodo del servicio y devolviendole datos del cliente', datos);
            dotnetReference.invokeMethodAsync('BlazorDBCallback', datos);
            }
        );

    },
    checkIsLogged: () => {
        let _reqDb = window.indexedDB.open('clientes');
        _reqDb.addEventListener('success', (ev) => {
            let _bd = _reqDb.result;

            if (_bd.objectStoreNames.contains('infoClientes')) {

                let _selectReq = _bd.transaction(['infoClientes'], 'readonly').objectStore('infoClientes').getAll();
                _selectReq.addEventListener('success', (evt2) => {
                    console.log('exito en checkislooged transaction getall..', _selectReq.result);

                    let _datos = _selectReq.result;
                    if (_datos.length > 0) {
                        return true;
                    } else {
                        return false;
                    }
                });
                _selectReq.addEventListener('error', (error) => {
                    console.log('errores al recuperar sesion cliente..', error);
                    return false;
                });
            } else {
                return false; //no existe el objectstore infoClientes, no se ha logueado...
            }
        });
        _reqDb.addEventListener('error', (errDB) => {
            console.log('error al abrir bd-clientes en indexedDB....', errDB);
            return false;
        });


    },
    almacenaClienteJWT: (datosCliente, token) => {
        let _reqDb = window.indexedDB.open('clientes');
        _reqDb.addEventListener('upgradeneeded', (ev) => {
            let _db = _reqDb.result;
            let _store = _db.createObjectStore('infoClientes', { keyPath: 'nif' });
            _store.createIndex('nif', 'nif');

            let _store2 = _db.createObjectStore('tokens', { autoIncrement: true });
            let _store3 = _db.createObjectStore('itemsPedidoActual', { autoIncrement: true });

        });

        _reqDb.addEventListener('success', (ev) => {
            //lanzo una transaccion sobre coleccion infoClientes para grabar documento "datosCliente"
            // y sobre tokens para grabar documento IToken

            let _db = _reqDb.result;
            let _transac = _db.transaction(['infoClientes','tokens'], 'readwrite');

            //1º operacion sobre transaccion...
            let _insertReq = _transac.objectStore('infoClientes').add(datosCliente);
            _insertReq.addEventListener('success', (ev) => console.log('datos almacenados del cliente ok en indexedDB'));
            _insertReq.addEventListener('error', (err) => { throw err });

            //2ºoperacion sobre transaccion...
            let _insertJWT = _transac.objectStore('tokens').add(token);
            _insertJWT.addEventListener('success', (ev) => console.log('datos almacenados del JWT ok en indexedDB'));
            _insertJWT.addEventListener('error', (err) => { throw err });
        });
    },
    almacenaItemsPedido: (items) => {
        let _reqDb = window.indexedDB.open('clientes');
        _reqDb.addEventListener('success', (ev) => {
            //lanzo una transaccion sobre coleccion infoClientes para grabar documento "datosCliente"
            // y sobre tokens para grabar documento IToken

            let _db = _reqDb.result;
            let _transac = _db.transaction(['itemsPedidoActual'], 'readwrite');

            let _insertReq = _transac.objectStore('itemsPedidoActual').add(items);
            _insertReq.addEventListener('success', (ev) => console.log('items pedido acutal almacenados del cliente ok en indexedDB'));
            _insertReq.addEventListener('error', (err) => { throw err });
        });

    },
    devuelveItemsPedido: (dotnetReference) => {
        var _promiseCli = new Promise(
            (resolve, reject) => {
                let _reqDb = window.indexedDB.open('clientes');
 
                _reqDb.addEventListener('success', (ev) => {
                    let _bd = _reqDb.result;

                    let _selectReq = _bd.transaction(['itemsPedidoActual'], 'readonly').objectStore('itemsPedidoActual').getAll();
                    _selectReq.addEventListener('success', (evt2) => {
                        let _datos = _selectReq.result;

                        console.log('datos recuperados itemsPedidoActual...', _datos[0]);
                        if (_datos[0] != undefined && _datos[0] != null) {
                            resolve(_datos[0]);
                        } else {
                            resolve(null);
                        }
                    });
                    _selectReq.addEventListener('error', (error) => {
                        console.log('errores al recuperar itemsPedidoActual..', error);
                        resolve(null);
                    });
                });
                _reqDb.addEventListener('error', (errDB) => {
                    console.log('error al abrir bd-clientes en indexedDB....', errDB);
                    resolve(null);
                });

            }
        ); //cierre promise

        _promiseCli.then(datos => {
            console.log('invocando desde js a metodo del servicio y devolviendole items del pedido actual', datos);
            dotnetReference.invokeMethodAsync('BlazorDBCallbackItems', datos);
        }
        );

    }
}