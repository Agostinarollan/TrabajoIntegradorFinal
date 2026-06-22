using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TrabajoPractico_Integrador.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Dni { get; set; }
        public string Email { get; set; }
        public int Telefono { get; set; }

        //relacion inversa
        public ICollection<Direccion> Direcciones{get;set;}
        public ICollection<Venta> Ventas{get;set;}

        public Cliente() { }
    }
}
