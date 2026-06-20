using System.ComponentModel.DataAnnotations.Schema;
namespace TrabajoPractico_Integrador.Models
{
    public class Rol
    {
        public int Id{ get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public Boolean Estado {  get; set; }    

        public ICollection<Usuario> Usuarios { get; set; }
        public Rol() { }


    }
}
