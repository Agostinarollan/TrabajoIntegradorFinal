using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrabajoPractico_Integrador.Data;
using TrabajoPractico_Integrador.Models;

namespace TrabajoPractico_Integrador.Controllers
{
    public class Detalle_ventaController : Controller
    {
        private readonly AppDbContext _context;

        public Detalle_ventaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Detalle_venta
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.DetalleVentas.Include(d => d.Producto).Include(d => d.Venta);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Detalle_venta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // El include hace el JOIN
            var detalle_venta = await _context.DetalleVentas
                .Include(d => d.Producto)
                .Include(d => d.Venta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalle_venta == null)
            {
                return NotFound();
            }

            return View(detalle_venta);
        }

        // GET: Detalle_venta/Create
        public IActionResult Create()
        {
            ViewData["ProductoID"] = new SelectList(_context.Productos, "Id", "Nombre");
            ViewData["VentaID"] = new SelectList(_context.Ventas, "Id", "Id");
            return View();
        }

        // POST: Detalle_venta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cantidad,PrecioUnitario,Subtotal,ProductoID,VentaID")] Detalle_venta detalle_venta)
        {
            if (ModelState.IsValid)
            {   //Busca en la tabla Productos el producto cuyo Id sea igual al ProductoID que elegí en el formulario de detalle venta.
                var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == detalle_venta.ProductoID);

                if (producto == null)
                {
                    return NotFound();
                }

                detalle_venta.PrecioUnitario = producto.PrecioVenta;

                detalle_venta.Subtotal = detalle_venta.Cantidad * detalle_venta.PrecioUnitario;

                _context.Add(detalle_venta);
                await _context.SaveChangesAsync();

                var venta = await _context.Ventas
                    .Include(v => v.Detalle_Ventas)
                    .FirstOrDefaultAsync(v => v.Id == detalle_venta.VentaID);

                if (venta != null)
                {
                    venta.Total = venta.Detalle_Ventas.Sum(d => d.Subtotal);
                    _context.Update(venta);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));


            }
            ViewData["ProductoID"] = new SelectList(_context.Productos, "Id", "Nombre", detalle_venta.ProductoID);
            ViewData["VentaID"] = new SelectList(_context.Ventas, "Id", "Id", detalle_venta.VentaID);
            return View(detalle_venta);
        }

        // GET: Detalle_venta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalle_venta = await _context.DetalleVentas.FindAsync(id);
            if (detalle_venta == null)
            {
                return NotFound();
            }
            ViewData["ProductoID"] = new SelectList(_context.Productos, "Id", "Nombre", detalle_venta.ProductoID);
            ViewData["VentaID"] = new SelectList(_context.Ventas, "Id", "Id", detalle_venta.VentaID);
            return View(detalle_venta);
        }

        // POST: Detalle_venta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cantidad,PrecioUnitario,Subtotal,ProductoID,VentaID")] Detalle_venta detalle_venta)
        {
            if (id != detalle_venta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalle_venta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Detalle_ventaExists(detalle_venta.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductoID"] = new SelectList(_context.Productos, "Id", "Nombre", detalle_venta.ProductoID);
            ViewData["VentaID"] = new SelectList(_context.Ventas, "Id", "Id", detalle_venta.VentaID);
            return View(detalle_venta);
        }

        // GET: Detalle_venta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalle_venta = await _context.DetalleVentas
                .Include(d => d.Producto)
                .Include(d => d.Venta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalle_venta == null)
            {
                return NotFound();
            }

            return View(detalle_venta);
        }

        // POST: Detalle_venta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detalle_venta = await _context.DetalleVentas.FindAsync(id);
            if (detalle_venta != null)
            {
                _context.DetalleVentas.Remove(detalle_venta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Detalle_ventaExists(int id)
        {
            return _context.DetalleVentas.Any(e => e.Id == id);
        }
    }
}
