using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Build.ObjectModelRemoting;
namespace TrabajoPractico_Integrador.Models
{
    public class Venta
    {
        public int Id { get; set; }
        public DateTime fecha { get; set; } = DateTime.Now;
        public decimal Total {  get; set; }
        public Boolean Estado { get; set; }
        public int ClienteID {  get; set; }
        public int UsuarioID { get; set; }
        public Cliente Cliente { get; set; }    
        public Usuario Usuario { get; set; }    
        public ICollection<Detalle_venta> Detalle_Ventas { get; set; }

        public Venta() { }
    }
}
