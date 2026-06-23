using System.ComponentModel.DataAnnotations.Schema;
namespace TrabajoPractico_Integrador.Models
{
    public class Detalle_venta
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public int ProductoID { get; set; }
        public int VentaID { get; set; }
        public Producto Producto { get; set; }

        public string Descripcion { get; set; }
        public Venta Venta { get; set; }
        public Detalle_venta() { }

    }
}
