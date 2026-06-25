using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrabajoPractico_Integrador.Data;
using TrabajoPractico_Integrador.Models;
using TrabajoPractico_Integrador.Models.ViewModels;

namespace TrabajoPractico_Integrador.Controllers
{
    public class VentasController : Controller
    {
        private readonly AppDbContext _context;

        public VentasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Ventas
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Ventas.Include(v => v.Cliente).Include(v => v.Usuario);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (venta == null)
                return NotFound();

            return View(venta);
        }

        // GET: Ventas/Create
        public IActionResult Create()
        {
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "Id", "Nombre");
            ViewData["UsuarioID"] = new SelectList(_context.Usuarios, "Id", "NombreUsuario"); // corregido: era "Nombre"
            ViewData["ProductoID"] = new SelectList(_context.Productos, "Id", "Descripcion");
            return View();
        }

        // POST: Ventas/Create (no se usa, el flujo va por ConfirmarPedido)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,fecha,Total,Estado,ClienteID,UsuarioID")] Venta venta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "Id", "Nombre", venta.ClienteID);
            ViewData["UsuarioID"] = new SelectList(_context.Usuarios, "Id", "NombreUsuario", venta.UsuarioID);
            ViewData["ProductoID"] = new SelectList(_context.Productos, "Id", "Descripcion");
            return View(venta);
        }

        // POST: Ventas/ConfirmarPedido
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarPedido(PedidoViewModel pedido)
        {
            // Validar que el cliente exista
            var cliente = await _context.Clientes.FindAsync(pedido.ClienteID);
            if (cliente == null)
            {
                ModelState.AddModelError("", "El cliente seleccionado no existe.");
                CargarViewData(pedido.ClienteID, pedido.UsuarioID);
                return View("Create", pedido);
            }

            // Validar que cada producto exista
            var idsProductos = pedido.Items
                .Select(i => i.ProductoID)
                .Distinct()
                .ToList();

            var productosDb = await _context.Productos
                .Where(p => idsProductos.Contains(p.Id))
                .ToListAsync();

            if (productosDb.Count != idsProductos.Count)
            {
                ModelState.AddModelError("", "Uno o más productos no existen en el sistema.");
                CargarViewData(pedido.ClienteID, pedido.UsuarioID);
                return View("Create", pedido);
            }

            // Si el producto se repite, sumar cantidades
            var itemsAgrupados = pedido.Items
                .GroupBy(i => i.ProductoID)
                .Select(g => new
                {
                    ProductoID = g.Key,
                    Cantidad = g.Sum(i => i.Cantidad)
                })
                .ToList();

            // Validar stock antes de guardar
            foreach (var item in itemsAgrupados)
            {
                var producto = productosDb.First(p => p.Id == item.ProductoID);
                if (producto.Stock < item.Cantidad)
                {
                    ModelState.AddModelError("",
                        $"Stock insuficiente para '{producto.Descripcion}'. Disponible: {producto.Stock}.");
                    CargarViewData(pedido.ClienteID, pedido.UsuarioID);
                    return View("Create", pedido);
                }
            }

            // Calcular detalles
            var detalles = itemsAgrupados.Select(item =>
            {
                var producto = productosDb.First(p => p.Id == item.ProductoID);
                var precioUnitario = producto.PrecioVenta;
                var subtotal = item.Cantidad * precioUnitario;

                return new Detalle_venta
                {
                    ProductoID = item.ProductoID,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = precioUnitario,
                    Subtotal = subtotal,
                    Descripcion = producto.Descripcion
                };
            }).ToList();

            var total = detalles.Sum(d => d.Subtotal);

            // Guardar Venta
            var venta = new Venta
            {
                ClienteID = pedido.ClienteID,
                UsuarioID = pedido.UsuarioID,
                fecha = DateTime.Now,
                Total = total,
                Estado = true
            };

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            // Guardar detalles y descontar stock
            foreach (var detalle in detalles)
            {
                detalle.VentaID = venta.Id;
                _context.DetalleVentas.Add(detalle);
            }

            foreach (var item in itemsAgrupados)
            {
                var producto = productosDb.First(p => p.Id == item.ProductoID);
                producto.Stock -= item.Cantidad;
                _context.Productos.Update(producto);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
                return NotFound();

            if (venta.Estado)
            {
                TempData["Error"] = "Este pedido ya fue confirmado y no puede editarse.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClienteID"] = new SelectList(_context.Clientes, "Id", "Nombre", venta.ClienteID);
            ViewData["UsuarioID"] = new SelectList(_context.Usuarios, "Id", "NombreUsuario", venta.UsuarioID);
            return View(venta);
        }

        // POST: Ventas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,fecha,Total,Estado,ClienteID,UsuarioID")] Venta venta)
        {
            if (id != venta.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaExists(venta.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClienteID"] = new SelectList(_context.Clientes, "Id", "Nombre", venta.ClienteID);
            ViewData["UsuarioID"] = new SelectList(_context.Usuarios, "Id", "NombreUsuario", venta.UsuarioID);
            return View(venta);
        }

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (venta == null)
                return NotFound();

            return View(venta);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta != null)
                _context.Ventas.Remove(venta);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.Id == id);
        }

        private void CargarViewData(int clienteID, int usuarioID)
        {
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "Id", "Nombre", clienteID);
            ViewData["UsuarioID"] = new SelectList(_context.Usuarios, "Id", "NombreUsuario", usuarioID);
            ViewData["ProductoID"] = new SelectList(_context.Productos, "Id", "Descripcion");
        }
    }
}