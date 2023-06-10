window.manageIndexedDB = {
    borrarDB: () => {
        let reqDelete = window.indexedDB.deleteDatabase('clientes')
        reqDelete.addEventListener('success', () => console.log('BD borrada con exito'))
        reqDelete.addEventListener('error', (err) => console.log('Error al borrar bd', err))
    },
    devuelveCliente: (dotnetReference) => {
        let promise = new Promise((resolve, reject) => {
            let reqDb = window.indexedDB.open('clientes')

                reqDb.addEventListener('upgradeneeded', (ev) => {
                    let db = reqDb.result
                    let store = db.createObjectStore('infoClientes', { keyPath: 'nif' })
                    store.createIndex('nif', 'nif')

                    db.createObjectStore('tokens', { autoIncrement: true })
                    db.createObjectStore('itemsPedidoActual', { autoIncrement: true })
                }) 
                reqDb.addEventListener('success', (ev) => {
                    let bd = reqDb.result
                    let selectReq = bd.transaction(['infoClientes'], 'readonly').objectStore('infoClientes').getAll()

                    selectReq.addEventListener('success', (evt2) => {
                        let datos = selectReq.result

                        if (datos[0] != undefined && datos[0] != null) {
                            resolve(datos[0])
                        } else {
                            resolve(null)
                        }
                    })
                    selectReq.addEventListener('error', (error) => { resolve(null) })
                }) 
                reqDb.addEventListener('error', (errDB) => { resolve(null) })
            }
        ) 
        promise.then(datos => {
            dotnetReference.invokeMethodAsync('BlazorDBCallback', datos)
        })
    },
    checkIsLogged: () => {
        let reqDb = window.indexedDB.open('clientes')
        reqDb.addEventListener('success', (ev) => {
            let bd = reqDb.result

            if (bd.objectStoreNames.contains('infoClientes')) {

                let selectReq = bd.transaction(['infoClientes'], 'readonly').objectStore('infoClientes').getAll()
                selectReq.addEventListener('success', (evt2) => {

                    let datos = selectReq.result
                    if (datos.length > 0) {
                        return true
                    } else {
                        return false
                    }
                })
                selectReq.addEventListener('error', (error) => { return false })
            } 
            return false 
        })
        reqDb.addEventListener('error', (errDB) => { return false })
    },
    almacenaClienteJWT: (datosCliente, token) => {
        let reqDb = window.indexedDB.open('clientes')
        reqDb.addEventListener('upgradeneeded', (ev) => {
            let db = reqDb.result
            let store = db.createObjectStore('infoClientes', { keyPath: 'nif' })
            store.createIndex('nif', 'nif')

            db.createObjectStore('tokens', { autoIncrement: true })
            db.createObjectStore('itemsPedidoActual', { autoIncrement: true })
        })
        reqDb.addEventListener('success', (ev) => {
            let db = _reqDb.result
            let tx = db.transaction(['infoClientes','tokens'], 'readwrite')

            let insertReq = tx.objectStore('infoClientes').add(datosCliente)
            insertReq.addEventListener('success', (ev) => console.log('datos almacenados del cliente ok en indexedDB'))
            insertReq.addEventListener('error', (err) => { throw err })

            let _insertJWT = tx.objectStore('tokens').add(token)
            insertJWT.addEventListener('success', (ev) => console.log('datos almacenados del JWT ok en indexedDB'))
            insertJWT.addEventListener('error', (err) => { throw err })
        })
    },
    almacenaItemsPedido: (items) => {
        let reqDb = window.indexedDB.open('clientes')
        reqDb.addEventListener('success', (ev) => {
            let db = _reqDb.result
            let tx = _db.transaction(['itemsPedidoActual'], 'readwrite')

            let insertReq = tx.objectStore('itemsPedidoActual').add(items)
            insertReq.addEventListener('success', (ev) => console.log('items pedido acutal almacenados del cliente ok en indexedDB'))
            insertReq.addEventListener('error', (err) => { throw err })
        })
    },
    devuelveItemsPedido: (dotnetReference) => {
        let promise = new Promise((resolve, reject) => {
            let reqDb = window.indexedDB.open('clientes')
 
            reqDb.addEventListener('success', (ev) => {
                let bd = reqDb.result
                let selectReq = bd.transaction(['itemsPedidoActual'], 'readonly').objectStore('itemsPedidoActual').getAll()
                    
                selectReq.addEventListener('success', (evt2) => {
                    let datos = selectReq.result

                    if (datos[0] != undefined && datos[0] != null) {
                        resolve(_datos[0])
                    } else {
                        resolve(null)
                    }
                })
                selectReq.addEventListener('error', (error) => { resolve(null) })
            })
            reqDb.addEventListener('error', (errDB) => { resolve(null) })
        }) 
        promise.then(datos => { dotnetReference.invokeMethodAsync('BlazorDBCallbackItems', datos) })
    }
}