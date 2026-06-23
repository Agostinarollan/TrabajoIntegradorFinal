
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrabajoPractico_Integrador.Models

{
    public class Producto
    { 
        public int Id { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Range(0, 999999)]
        public int Stock { get; set; }
        [Range(0, 999999)]
        public decimal PrecioVenta { get; set; }
        public ICollection <Detalle_venta> Detalles { get; set; } = new Collection<Detalle_venta>();
        public Producto() {
        }


            
                

    }
}
