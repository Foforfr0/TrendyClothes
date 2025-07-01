using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System;

namespace ClienteMAUI.Models.ViewModel
{
    public class ProductoViewModel
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? Categoria { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public ImageSource? ImageSource { get; set; }
        public bool IsOwnedByUser { get; set; }

        // Constructor vacío para binding
        public ProductoViewModel() { }

        // Constructor con parámetros
        public ProductoViewModel(int id, string nombre, string descripcion, decimal precio, int stock, string categoria, ImageSource imageSource, bool isOwnedByUser)
        {
            Id = id;
            Nombre = nombre;
            Descripcion = descripcion;
            Precio = precio;
            Stock = stock;
            Categoria = categoria;
            ImageSource = imageSource;
            IsOwnedByUser = isOwnedByUser;
        }

        public override string ToString()
        {
            return $"{Nombre} - {Descripcion} - Precio: {Precio:C} - Stock: {Stock}";
        }
    }
}

