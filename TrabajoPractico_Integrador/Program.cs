using Microsoft.EntityFrameworkCore;
using TrabajoPractico_Integrador.Data;
using TrabajoPractico_Integrador.Models;

namespace TrabajoPractico_Integrador
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddRazorPages();
            // Sesión
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });


            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
                if (!db.Roles.Any())
                {
                    db.Roles.AddRange(
                        new Rol { Nombre = "Administrador", Descripcion = "Acceso total", Estado = true },
                        new Rol { Nombre = "Vendedor", Descripcion = "Registra ventas", Estado = true }
                    );
                    db.SaveChanges();
                }

                if (!db.Usuarios.Any())
                {
                    var rolAdmin = db.Roles.First(r => r.Nombre == "Administrador");
                    db.Usuarios.Add(new Usuario
                    {
                        NombreUsuario = "admin",
                        Contraseña = BCrypt.Net.BCrypt.HashPassword("Admin123"),
                        Estado = true,
                        IdRol = rolAdmin.Id
                    });
                    db.SaveChanges();

                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
