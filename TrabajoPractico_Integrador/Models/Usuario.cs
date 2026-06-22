using System.ComponentModel.DataAnnotations.Schema;
namespace TrabajoPractico_Integrador.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }    
        public string Contraseña { get; set; }
        public Boolean Estado { get; set; } = true;
        public int IdRol {  get; set; }
        public Rol Rol { get; set; }
        
        public ICollection<Venta> Ventas { get; set; }

        public Usuario()
        {

        }
    }
}
