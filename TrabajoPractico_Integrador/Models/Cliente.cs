using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrabajoPractico_Integrador.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(50)]
        public string Apellido { get; set; }
        [Required]
        public int Dni { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public int Telefono { get; set; }
        //relacion inversa
        public ICollection<Direccion> Direcciones{get;set;}
        public ICollection<Venta> Ventas{get;set;}

        public Cliente() { }
    }
}
