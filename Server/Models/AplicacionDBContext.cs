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
        public DbSet<Pedido> PedidosCliente { get; set; }
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
            builder.Entity<Materia>().HasKey((Materia mat) => mat.IdMateria);
            builder.Entity<Materia>().Property((Materia mat) => mat.IdMateriaPadre).IsRequired();
            builder.Entity<Materia>().Property((Materia mat) => mat.NombreMateria).IsRequired();

            builder.Entity<ItemPedido>().ToTable("ItemsPedido");
            builder.Entity<ItemPedido>().HasKey((ItemPedido itemp) => itemp.ItemPedidoId);
            builder.Entity<ItemPedido>().Property((ItemPedido itemp) => itemp.PedidoId).IsRequired();
            builder.Entity<ItemPedido>().Property((ItemPedido itemp) => itemp.Cantidad).IsRequired();
            builder.Entity<ItemPedido>().Property((ItemPedido itemp) => itemp.LibroPedidoId).IsRequired();

            builder.Entity<Pedido>().ToTable("PedidosCliente");
            builder.Entity<Pedido>().HasKey((Pedido ped) => ped.PedidoId);
            builder.Entity<Pedido>().Property((Pedido ped) => ped.EstadoPedido).IsRequired();
            builder.Entity<Pedido>().Property((Pedido ped) => ped.FechaPedido).IsRequired();
            builder.Entity<Pedido>().Property((Pedido ped) => ped.ClienteId).IsRequired();

            builder.Entity<Pedido>().Property((Pedido ped) => ped.ElementosPedido).HasConversion(
                (List<String> elemlista) => String.Join(":", elemlista), (String valorDB) => valorDB.Split(":", StringSplitOptions.RemoveEmptyEntries).ToList<String>()
            );
            builder.Entity<Pedido>().Property((Pedido ped) => ped.GastosEnvio).IsRequired().HasColumnType("DECIMAL(5,2)");
            builder.Entity<Pedido>().Property((Pedido ped) => ped.SubTotalPedido).IsRequired().HasColumnType("DECIMAL(5,2)");
            builder.Entity<Pedido>().Property((Pedido ped) => ped.TotalPedido).IsRequired().HasColumnType("DECIMAL(5,2)");

            builder.Entity<Libro>().ToTable("Libros").HasKey((Libro lib) => lib.LibroId);
            builder.Entity<Libro>().Property((Libro lib) => lib.Titulo);
            builder.Entity<Libro>().Property((Libro lib) => lib.Autor);
            builder.Entity<Libro>().Property((Libro lib) => lib.Descripcion);
            builder.Entity<Libro>().Property((Libro lib) => lib.Editorial);
            builder.Entity<Libro>().Property((Libro lib) => lib.FicheroImagen);
            builder.Entity<Libro>().Property((Libro lib) => lib.IdMateria).IsRequired();
            builder.Entity<Libro>().Property((Libro lib) => lib.ISBN10).IsRequired().HasMaxLength(20);
            builder.Entity<Libro>().Property((Libro lib) => lib.ISBN13).HasMaxLength(20);
            builder.Entity<Libro>().Property((Libro lib) => lib.NumeroPaginas);
            builder.Entity<Libro>().Property((Libro lib) => lib.Precio).IsRequired().HasColumnType("DECIMAL(5,2)");

            builder.Entity<Direccion>().ToTable("Direcciones");
            builder.Entity<Direccion>().HasKey((Direccion direc) => direc.DireccionId);
            builder.Entity<Direccion>().Property((Direccion direc) => direc.ClienteId).IsRequired();
            builder.Entity<Direccion>().Property((Direccion direc) => direc.Calle).IsRequired();
            builder.Entity<Direccion>().Property((Direccion direc) => direc.CP).IsRequired().HasMaxLength(5);
            builder.Entity<Direccion>().Property((Direccion direc) => direc.CodPro).IsRequired();
            builder.Entity<Direccion>().Property((Direccion direc) => direc.CodMun).IsRequired();
            builder.Entity<Direccion>().Property((Direccion direc) => direc.EsPrincipal).IsRequired();
            builder.Entity<Direccion>().Property((Direccion direc) => direc.TipoDireccion).IsRequired();

            builder.Entity<Provincia>().ToTable("Provincias");
            builder.Entity<Provincia>().HasKey((Provincia prov) => prov.CodPro);
            builder.Entity<Provincia>().Property((Provincia prov) => prov.CodPro).IsRequired();
            builder.Entity<Provincia>().Property((Provincia prov) => prov.NombreProvincia).IsRequired();

            builder.Entity<Municipio>().ToTable("Municipios");
            builder.Entity<Municipio>().HasKey((Municipio muni) => new { muni.CodPro, muni.CodMun });
            builder.Entity<Municipio>().Property((Municipio muni) => muni.CodPro).IsRequired();
            builder.Entity<Municipio>().Property((Municipio muni) => muni.CodMun).IsRequired();
            builder.Entity<Municipio>().Property((Municipio muni) => muni.NombreMunicipio).IsRequired();
        }
    }
}
