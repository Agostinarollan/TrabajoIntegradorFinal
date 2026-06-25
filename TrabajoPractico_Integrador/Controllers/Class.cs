using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabajoPractico_Integrador.Data;
using TrabajoPractico_Integrador.Models.ViewModels;

namespace TrabajoPractico_Integrador.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Login
        public IActionResult Index()
        {
            // Si ya hay sesión activa, ir directo al home
            if (HttpContext.Session.GetString("UsuarioNombre") != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: /Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
                return View(modelo);

            // Buscar usuario activo con ese nombre
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.NombreUsuario == modelo.NombreUsuario && u.Estado);

            // Verificar que exista y que la contraseña sea correcta
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(modelo.Contraseña, usuario.Contraseña))
            {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                return View(modelo);
            }

            // Guardar datos en sesión
            HttpContext.Session.SetInt32("UsuarioID", usuario.Id);
            HttpContext.Session.SetString("UsuarioNombre", usuario.NombreUsuario);
            HttpContext.Session.SetString("UsuarioRol", usuario.Rol?.Nombre ?? "");

            return RedirectToAction("Index", "Home");
        }

        // GET: /Login/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}