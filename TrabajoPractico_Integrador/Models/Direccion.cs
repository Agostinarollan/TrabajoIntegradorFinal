using System.ComponentModel.DataAnnotations.Schema;
namespace TrabajoPractico_Integrador.Models
{
    public class Direccion
    {
        public int Id { get; set; }
        public string Calle { get; set; }

        public int Numero { get; set; }

        public string Ciudad { get; set; }

        public int CodigoPostal { get; set; }

        //clave foranea
        public int IdCliente { get; set; }

        //propiedad de navegacion
        public Cliente Cliente { get; set; }


        public Direccion()
        {

        }
    }
}
