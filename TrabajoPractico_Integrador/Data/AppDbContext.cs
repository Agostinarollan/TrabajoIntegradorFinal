
using TrabajoPractico_Integrador.Models;
using Microsoft.EntityFrameworkCore;
namespace TrabajoPractico_Integrador.Data

{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
             : base(options)
        {
        }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Direccion> Direcciones { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Detalle_venta> DetalleVentas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Direcciones)
                .WithOne(d => d.Cliente)
                .HasForeignKey(d => d.ClienteID);

            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Ventas)
                .WithOne(v => v.Cliente)
                .HasForeignKey(v => v.ClienteID);

            modelBuilder.Entity<Venta>()
                .HasMany(v => v.Detalle_Ventas)
                .WithOne(dt => dt.Venta)
                .HasForeignKey(dt => dt.VentaID);

            modelBuilder.Entity<Producto>()
              .HasMany(p => p.Detalles)
              .WithOne(dt => dt.Producto)
              .HasForeignKey(dt =>dt.ProductoID);

            modelBuilder.Entity<Usuario>()
              .HasMany(u => u.Ventas)
              .WithOne(v => v.Usuario)
              .HasForeignKey(v => v.UsuarioID);

            modelBuilder.Entity<Rol>()
              .HasMany(r => r.Usuarios)
              .WithOne(u => u.Rol)
              .HasForeignKey(u => u.IdRol);



        }

    }
}
