using Shared.DTOs;

namespace BlazorApp.MVVM.Models
{
    public class CartItem
    {
        public ProductosDTO Product { get; set; } = null!;
        public int Quantity { get; set; }

        public decimal Subtotal => (decimal)(Product.Price * Quantity);
        public int EarnedPoints => (int)(Subtotal / 10); // Ejemplo: 1 punto por cada 10 unidades monetarias
    }
}
