using bookstore.Server.Models;
using bookstore.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace bookstore.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RESTTiendaServiceController : ControllerBase
    {
        private AplicacionDBContext _db;

        public RESTTiendaServiceController(AplicacionDBContext dbContextDI)
        {
            this._db = dbContextDI;
        }


        [HttpGet]
        public RESTMessage DevolverLibros([FromQuery] int idCategoria)
        {
            try
            {
                List<Libro> libros = this._db.Libros.Where((Libro libro) => libro.IdMateria == idCategoria).ToList<Libro>();

                return new RESTMessage
                {
                    Datos = libros,
                    DatosCliente = null,
                    Errores = null,
                    Mensaje = "Libros recuperados ok"
                };
            }
            catch (Exception ex)
            {
                return new RESTMessage
                {
                    Datos = null,
                    DatosCliente = null,
                    Errores = new List<String> { ex.Message },
                    Mensaje = "Error al recuperar libros de la categoria: " + idCategoria
                };
            }
        }

        [HttpGet]
        public RESTMessage DevolverMaterias([FromQuery] int idMateria)
        {
            try
            {
                List<Materia> materias = this._db.Materias.Where((Materia materia) => materia.IdMateriaPadre == idMateria).ToList<Materia>();

                return new RESTMessage
                {
                    Datos = materias,
                    DatosCliente = null,
                    Errores = null,
                    Mensaje = "Materias recuperadas ok"
                };
            }
            catch (Exception ex)
            {
                return new RESTMessage
                {
                    Datos = null,
                    DatosCliente = null,
                    Errores = new List<String> { ex.Message },
                    Mensaje = "Error al recuperar materias con id: " + idMateria
                };
            }
        }

        [HttpGet]
        public RESTMessage RecuperaLibro([FromQuery] String idLibro)
        {
            try
            {
                Libro libro = this._db.Libros.Where((Libro libro) => libro.LibroId == idLibro).Single<Libro>();

                return new RESTMessage
                {
                    Datos = libro,
                    DatosCliente = null,
                    Errores = null,
                    Mensaje = "Libro recuperado ok"
                };
            }
            catch (Exception ex)
            {
                return new RESTMessage
                {
                    Datos = null,
                    DatosCliente = null,
                    Errores = new List<String> { ex.Message },
                    Mensaje = "Error al recuperar materias con id: " + idLibro
                };
            }
        }
    }
}
