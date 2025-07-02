using Microsoft.Maui.Controls;

namespace ClienteMAUI.Models.ViewModel
{
    public class ProductoViewModel
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public decimal Precio { get; set; }
        public int CantidadVendidos { get; set; }
        public ImageSource? ImageSource { get; set; }

        public bool EsPropio { get; set; }

        public ProductoViewModel() { }

        public ProductoViewModel(int id, string nombre, decimal precio, int cantidadVendidos, ImageSource? imageSource, bool esPropio)
        {
            Nombre = nombre;
            Precio = precio;
            CantidadVendidos = cantidadVendidos;
            ImageSource = imageSource;
            EsPropio = esPropio;
        }

        public override string ToString()
        {
            return $"{Nombre} - Precio: {Precio:C} - Vendidos: {CantidadVendidos}";
        }
    }
}
