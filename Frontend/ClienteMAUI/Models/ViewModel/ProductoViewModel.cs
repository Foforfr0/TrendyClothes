using Microsoft.Maui.Controls;

namespace ClienteMAUI.Models.ViewModel
{
    public class ProductoViewModel
    {
        public string? Nombre { get; set; }
        public decimal Precio { get; set; }
        public int CantidadVendidos { get; set; }
        public ImageSource? ImageSource { get; set; }

        // Constructor vacío para binding
        public ProductoViewModel() { }

        // Constructor con parámetros
        public ProductoViewModel(string nombre, decimal precio, int cantidadVendidos, ImageSource? imageSource)
        {
            Nombre = nombre;
            Precio = precio;
            CantidadVendidos = cantidadVendidos;
            ImageSource = imageSource;
        }

        public override string ToString()
        {
            return $"{Nombre} - Precio: {Precio:C} - Vendidos: {CantidadVendidos}";
        }
    }
}
