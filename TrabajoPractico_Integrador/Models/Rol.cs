using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TrabajoPractico_Integrador.Models
{
    public class Rol
    {
        public int Id{ get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        [StringLength(200)]
        public string Descripcion { get; set; } 
        public Boolean Estado {  get; set; } =true;   

        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public Rol() { }


    }
}
