using System.ComponentModel.DataAnnotations.Schema;
namespace TrabajoPractico_Integrador.Models
{
    public class Detalle_venta
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public int PrecioUnitario { get; set; }
        public int Subtotal { get; set; }
        public int ProductoID { get; set; }
        public int VentaID { get; set; }
        public Producto Producto { get; set; }

        public Venta Venta { get; set; }
        public Detalle_venta() { }

    }
}
