using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TrabajoPractico_Integrador.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string NombreUsuario { get; set; }
        [Required]
        [StringLength(50)]
        public string Contraseña { get; set; }
        public Boolean Estado { get; set; } = true;
        public int IdRol {  get; set; }
        public Rol Rol { get; set; }
        
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();

        public Usuario()
        {

        }
    }
}
