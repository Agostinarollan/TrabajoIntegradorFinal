namespace TrabajoPractico_Integrador.Models.ViewModels
{
   
    public class PedidoViewModel
    {
        public int ClienteID { get; set; }
        public int UsuarioID { get; set; }
        public List<ItemPedido> Items { get; set; } = new List<ItemPedido>();
    }

    public class ItemPedido
    {
        public int ProductoID { get; set; }
        public int Cantidad { get; set; }
    }
    
}
