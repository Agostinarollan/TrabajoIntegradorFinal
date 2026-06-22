
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace TrabajoPractico_Integrador.Models
{
    public class Producto
    { 
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int Stock { get; set; }
        public decimal PrecioVenta { get; set; }
        public ICollection <Detalle_venta> Detalles { get; set; }
        public Producto() {
        }


            
                

    }
}
