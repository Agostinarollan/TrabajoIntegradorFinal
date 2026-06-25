using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrabajoPractico_Integrador.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [Display(Name = "Nombre de usuario")]
        public string? NombreUsuario { get; set; }

        // Sin [Required] — el controller lo valida manualmente para poder hashearla
        [Display(Name = "Contraseña")]
        public string? Contraseña { get; set; }

        [Display(Name = "Estado")]
        public Boolean Estado { get; set; } = true;

        public int IdRol { get; set; }

        // Debe ser nullable para que el ModelState no falle al crear
        public Rol? Rol { get; set; }

        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();

        public Usuario() { }
    }
}