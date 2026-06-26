using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrabajoPractico_Integrador.Data;
using TrabajoPractico_Integrador.Filters;
using TrabajoPractico_Integrador.Models;

namespace TrabajoPractico_Integrador.Controllers
{
    [SoloAdmin]
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Usuarios.Include(u => u.Rol);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["IdRol"] = new SelectList(_context.Roles, "Id", "Nombre");
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreUsuario,Contraseña,Estado,IdRol")] Usuario usuario)
        {
            // Quitar la validación automática de Contraseña para manejarla manualmente
            ModelState.Remove("Contraseña");

            if (string.IsNullOrWhiteSpace(usuario.Contraseña))
            {
                ModelState.AddModelError("Contraseña", "La contraseña es obligatoria.");
            }

            if (ModelState.IsValid)
            {
                // Hashear la contraseña antes de guardar
                usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(usuario.Contraseña);

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdRol"] = new SelectList(_context.Roles, "Id", "Nombre", usuario.IdRol);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            // No enviar el hash a la vista
            usuario.Contraseña = string.Empty;

            ViewData["IdRol"] = new SelectList(_context.Roles, "Id", "Nombre", usuario.IdRol);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreUsuario,Contraseña,Estado,IdRol")] Usuario usuario)
        {
            if (id != usuario.Id)
                return NotFound();

            // Quitar validación automática de Contraseña (puede venir vacía si no se cambia)
            ModelState.Remove("Contraseña");

            if (ModelState.IsValid)
            {
                try
                {
                    // Buscar el usuario actual para recuperar el hash existente
                    var usuarioExistente = await _context.Usuarios
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.Id == id);

                    if (usuarioExistente == null)
                        return NotFound();

                    if (string.IsNullOrWhiteSpace(usuario.Contraseña))
                    {
                        // Si no ingresó nueva contraseña, conservar la anterior
                        usuario.Contraseña = usuarioExistente.Contraseña;
                    }
                    else
                    {
                        // Si ingresó una nueva, hashearla
                        usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(usuario.Contraseña);
                    }

                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["IdRol"] = new SelectList(_context.Roles, "Id", "Nombre", usuario.IdRol);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}