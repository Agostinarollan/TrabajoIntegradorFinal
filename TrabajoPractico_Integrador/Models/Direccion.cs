using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TrabajoPractico_Integrador.Models
{
    public class Direccion
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Calle { get; set; }

        [Required]
        public int Numero { get; set; }

        [Required]
        [StringLength(50)]
        public string Ciudad { get; set; }

        [Required]
        public int CodigoPostal { get; set; }

        // Definimos la clave foranea
        public int ClienteID { get; set; }

        //propiedad de navegacion
        public Cliente Cliente { get; set; }

        //constructor vacio 
        public Direccion()
        {

        }
    }
}
