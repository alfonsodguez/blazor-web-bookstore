window.manageIndexedDB = {
    borrarIndexedDB: () => {
        let reqDelete = window.indexedDB.deleteDatabase('clientes')
        reqDelete.addEventListener('success', () => console.log('BD borrada con exito'))
        reqDelete.addEventListener('error', (err) => console.log('Error al borrar bd', err))
    },
    devolverCliente: (dotnetReference) => {
        const promise = new Promise((resolve, reject) => {
            const reqDb = window.indexedDB.open('clientes')

            reqDb.addEventListener('upgradeneeded', (ev) => {
                const db = reqDb.result
                const store = db.createObjectStore('infoClientes', { keyPath: 'nif' })
                store.createIndex('nif', 'nif')

                db.createObjectStore('tokens', { autoIncrement: true })
                db.createObjectStore('itemsPedidoActual', { autoIncrement: true })
            })
            reqDb.addEventListener('success', (ev) => {
                const bd = reqDb.result
                const selectReq = bd.transaction(['infoClientes'], 'readonly').objectStore('infoClientes').getAll()

                selectReq.addEventListener('success', (evt2) => {
                    const datos = selectReq.result

                    if (datos[0] != undefined && datos[0] != null) {
                        resolve(datos[0])
                    } else {
                        resolve(null)
                    }
                })
                selectReq.addEventListener('error', (error) => { resolve(null) })
            })
            reqDb.addEventListener('error', (errDB) => { resolve(null) })
        })
        promise.then(datos => { dotnetReference.invokeMethodAsync('BlazorDBCallback', datos) })
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
                    }
                    return false
                })
                selectReq.addEventListener('error', (error) => { return false })
            }
            return false
        })
        reqDb.addEventListener('error', (errDB) => { return false })
    },
    almacenarClienteJWT: (datosCliente, token) => {
        let reqDb = window.indexedDB.open('clientes')
        reqDb.addEventListener('upgradeneeded', (ev) => {
            let db = reqDb.result
            let store = db.createObjectStore('infoClientes', { keyPath: 'nif' })
            store.createIndex('nif', 'nif')

            db.createObjectStore('tokens', { autoIncrement: true })
            db.createObjectStore('itemsPedidoActual', { autoIncrement: true })
        })
        reqDb.addEventListener('success', (ev) => {
            let db = reqDb.result
            let tx = db.transaction(['infoClientes', 'tokens'], 'readwrite')

            let insertReq = tx.objectStore('infoClientes').add(datosCliente)
            insertReq.addEventListener('success', (ev) => console.log('datos almacenados del cliente ok en indexedDB'))
            insertReq.addEventListener('error', (err) => { throw err })

            let insertJWT = tx.objectStore('tokens').add(token)
            insertJWT.addEventListener('success', (ev) => console.log('datos almacenados del JWT ok en indexedDB'))
            insertJWT.addEventListener('error', (err) => { throw err })
        })
    },
    almacenarItemsPedido: (items) => {
        let reqDb = window.indexedDB.open('clientes')
        reqDb.addEventListener('success', (ev) => {
            let db = reqDb.result
            let tx = db.transaction(['itemsPedidoActual'], 'readwrite')

            let insertReq = tx.objectStore('itemsPedidoActual').add(items)
            insertReq.addEventListener('success', (ev) => console.log('items pedido acutal almacenados del cliente ok en indexedDB'))
            insertReq.addEventListener('error', (err) => { throw err })
        })
    },
    devolverItemsPedido: (dotnetReference) => {
        let promise = new Promise((resolve, reject) => {
            let reqDb = window.indexedDB.open('clientes')

            reqDb.addEventListener('success', (ev) => {
                let bd = reqDb.result
                let selectReq = bd.transaction(['itemsPedidoActual'], 'readonly').objectStore('itemsPedidoActual').getAll()

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
        })
        promise.then(datos => { dotnetReference.invokeMethodAsync('BlazorDBCallbackItemsPedido', datos) })
    }
}