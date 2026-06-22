using System.ComponentModel.DataAnnotations.Schema;
namespace TrabajoPractico_Integrador.Models
{
    public class Rol
    {
        public int Id{ get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; } 
        public Boolean Estado {  get; set; } =true;   

        public ICollection<Usuario> Usuarios { get; set; }
        public Rol() { }


    }
}
