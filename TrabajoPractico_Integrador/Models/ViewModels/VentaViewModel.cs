namespace TrabajoPractico_Integrador.Models.ViewModels
{
    public class VentaViewModel
    {
        public int ClienteId { get; set; }
        public int UsuarioId { get; set; }

        public List<ItemVentaViewModel> Items { get; set; } = new List<ItemVentaViewModel>();

        public decimal TotalGeneral => Items.Sum(i => i.Subtotal);

    }
    public class ItemVentaViewModel
    {
        public int ProductoId { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}
