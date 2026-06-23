using Microsoft.Build.ObjectModelRemoting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TrabajoPractico_Integrador.Models
{
    public class Venta
    {
        public int Id { get; set; }
        public DateTime fecha { get; set; } = DateTime.Now;
        [Range(0, 999999999)]
        public decimal Total {  get; set; }
        public Boolean Estado { get; set; }
        public int ClienteID {  get; set; }
        public int UsuarioID { get; set; }
        public Cliente Cliente { get; set; }    
        public Usuario Usuario { get; set; }    
        public ICollection<Detalle_venta> Detalle_Ventas { get; set; } = new List<Detalle_venta>();

        public Venta() { }
    }
}
