using BlazorApp.MVVM.Models;
using Shared.DTOs;

namespace BlazorApp.MVVM.Services
{
    public interface IShoppingCartService
    {
        string UserName { get; set; } // Nombre de usuario del cliente
        List<CartItem> Items { get; } // Lista de productos en el carrito
        int AvailablePoints { get; set; } // Puntos que el usuario puede canjear en la compra
        int PointsToRedeem { get; set; }   // Puntos que el usuario desea canjear

        event Action OnChange; // Evento para notificar cambios en el carrito

        void AddProduct(ProductosDTO product, int quantity = 1);
        void RemoveProduct(int productId);
        void UpdateQuantity(int productId, int newQuantity);
        void Clear();
        decimal GetSubTotal();
        decimal GetDiscount();
        decimal GetTotal();
        int GetTotalEarnedPoints();
    }
}
