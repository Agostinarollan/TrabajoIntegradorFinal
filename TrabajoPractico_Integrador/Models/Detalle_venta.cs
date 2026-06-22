using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TrabajoPractico_Integrador.Models
{
    public class Detalle_venta
    {
        public int Id { get; set; }
        [Range(1, 9999)]
        public int Cantidad { get; set; }
        [Range(0.01, 999999)]
        public decimal PrecioUnitario { get; set; }
        [Range(0.01, 999999999)]
        public decimal Subtotal { get; set; }
        public int ProductoID { get; set; }
        public int VentaID { get; set; }
        public Producto Producto { get; set; }

        public Venta Venta { get; set; }
        public Detalle_venta() { }

    }
}
