using bookstore.Shared;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace bookstore.Server.Models
{
    public class AplicacionDBContext : IdentityDbContext
    {
        public DbSet<Direccion> Direcciones { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItemsPedido { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Materia> Materias { get; set; }

        public AplicacionDBContext(DbContextOptions<AplicacionDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ClienteIdentity>();

            builder.Entity<Materia>().ToTable("Materias");
            builder.Entity<Materia>().HasKey((Materia materia) => materia.MateriaId);
            builder.Entity<Materia>().Property((Materia materia) => materia.MateriaPadreId).IsRequired();
            builder.Entity<Materia>().Property((Materia materia) => materia.Nombre).IsRequired();

            builder.Entity<ItemPedido>().ToTable("ItemsPedido");
            builder.Entity<ItemPedido>().HasKey((ItemPedido item) => item.ItemPedidoId);
            builder.Entity<ItemPedido>().Property((ItemPedido item) => item.PedidoId).IsRequired();
            builder.Entity<ItemPedido>().Property((ItemPedido item) => item.Cantidad).IsRequired();
            builder.Entity<ItemPedido>().Property((ItemPedido item) => item.LibroPedidoId).IsRequired();

            builder.Entity<Pedido>().ToTable("Pedidos");
            builder.Entity<Pedido>().HasKey((Pedido pedido) => pedido.PedidoId);
            builder.Entity<Pedido>().Property((Pedido pedido) => pedido.ClienteId).IsRequired();
            builder.Entity<Pedido>().Property((Pedido pedido) => pedido.Estado).IsRequired();
            builder.Entity<Pedido>().Property((Pedido pedido) => pedido.Fecha).IsRequired();
            builder.Entity<Pedido>().Property((Pedido pedido) => pedido.ElementosPedido).HasConversion(
                (List<String> items) => String.Join(":", items), (String valorDB) => valorDB.Split(":", StringSplitOptions.RemoveEmptyEntries).ToList<String>());
            builder.Entity<Pedido>().Property((Pedido pedido) => pedido.GastosEnvio).IsRequired().HasColumnType("DECIMAL(5,2)");
            builder.Entity<Pedido>().Property((Pedido pedido) => pedido.SubTotal).IsRequired().HasColumnType("DECIMAL(5,2)");
            builder.Entity<Pedido>().Property((Pedido pedido) => pedido.Total).IsRequired().HasColumnType("DECIMAL(5,2)");

            builder.Entity<Libro>().ToTable("Libros").HasKey((Libro libro) => libro.LibroId);
            builder.Entity<Libro>().Property((Libro libro) => libro.Titulo);
            builder.Entity<Libro>().Property((Libro libro) => libro.Autor);
            builder.Entity<Libro>().Property((Libro libro) => libro.Descripcion);
            builder.Entity<Libro>().Property((Libro libro) => libro.Editorial);
            builder.Entity<Libro>().Property((Libro libro) => libro.FicheroImagen);
            builder.Entity<Libro>().Property((Libro libro) => libro.MateriaId).IsRequired();
            builder.Entity<Libro>().Property((Libro libro) => libro.ISBN10).IsRequired().HasMaxLength(20);
            builder.Entity<Libro>().Property((Libro libro) => libro.ISBN13).HasMaxLength(20);
            builder.Entity<Libro>().Property((Libro libro) => libro.NumeroPaginas);
            builder.Entity<Libro>().Property((Libro libro) => libro.Precio).IsRequired().HasColumnType("DECIMAL(5,2)");

            builder.Entity<Direccion>().ToTable("Direcciones");
            builder.Entity<Direccion>().HasKey((Direccion direccion) => direccion.DireccionId);
            builder.Entity<Direccion>().Property((Direccion direccion) => direccion.ClienteId).IsRequired();
            builder.Entity<Direccion>().Property((Direccion direccion) => direccion.Calle).IsRequired();
            builder.Entity<Direccion>().Property((Direccion direccion) => direccion.CP).IsRequired().HasMaxLength(5);
            builder.Entity<Direccion>().Property((Direccion direccion) => direccion.CodProvincia).IsRequired();
            builder.Entity<Direccion>().Property((Direccion direccion) => direccion.CodMunicipio).IsRequired();
            builder.Entity<Direccion>().Property((Direccion direccion) => direccion.EsPrincipal).IsRequired();
            builder.Entity<Direccion>().Property((Direccion direccion) => direccion.Tipo).IsRequired();

            builder.Entity<Provincia>().ToTable("Provincias");
            builder.Entity<Provincia>().HasKey((Provincia provincia) => provincia.CodProvincia);
            builder.Entity<Provincia>().Property((Provincia provincia) => provincia.CodProvincia).IsRequired();
            builder.Entity<Provincia>().Property((Provincia provincia) => provincia.NombreProvincia).IsRequired();

            builder.Entity<Municipio>().ToTable("Municipios");
            builder.Entity<Municipio>().HasKey((Municipio municipio) => new { municipio.CodProvincia, municipio.CodMunicipio });
            builder.Entity<Municipio>().Property((Municipio municipio) => municipio.CodProvincia).IsRequired();
            builder.Entity<Municipio>().Property((Municipio municipio) => municipio.CodMunicipio).IsRequired();
            builder.Entity<Municipio>().Property((Municipio municipio) => municipio.Nombre).IsRequired();
        }
    }
}
