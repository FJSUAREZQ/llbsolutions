using BlazorApp.MVVM.Models;
using Shared.DTOs;

namespace BlazorApp.MVVM.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        public string UserName { get; set; } = "Anónimo";

        private readonly List<CartItem> _items = new();// Lista de productos en el carrito
        public List<CartItem> Items => _items;// Exposición de la lista de productos en el carrito
        public int AvailablePoints { get; set; } = 0;//Puntos que el usuario puede canjear en la compra
        public int PointsToRedeem { get; set; } //Puntos que el usuario desea canjear por descuento

        public event Action? OnChange;

        /// <summary>
        /// Agrega un producto al carrito de compras. Si el producto ya existe, incrementa la cantidad.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        public void AddProduct(ProductosDTO product, int quantity = 1)
        {
            var item = _items.FirstOrDefault(i => i.Product.Id == product.Id);
            if (item == null)
            {
                _items.Add(new CartItem { Product = product, Quantity = quantity });
            }
            else
            {
                item.Quantity += quantity;
            }

            NotifyStateChanged();
        }

        /// <summary>
        /// Elimina un producto del carrito de compras por su ID.
        /// Este se ejecuta al dar clic en el botón "Eliminar" del producto en el carrito.
        /// </summary>
        /// <param name="productId"></param>
        public void RemoveProduct(int productId)
        {
            var item = _items.FirstOrDefault(i => i.Product.Id == productId);
            if (item != null)
            {
                _items.Remove(item);
                NotifyStateChanged();
            }
        }

        /// <summary>
        /// Actualiza la cantidad de un producto en el carrito de compras.
        /// Esta se ejecuta al cambiar la cantidad en el input del producto en el carrito.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="newQuantity"></param>
        public void UpdateQuantity(int productId, int newQuantity)
        {
            var item = _items.FirstOrDefault(i => i.Product.Id == productId);
            if (item != null && newQuantity > 0)
            {
                item.Quantity = newQuantity;
                NotifyStateChanged();
            }
        }

        /// <summary>
        /// Limpia el carrito de compras, eliminando todos los productos y restableciendo los puntos a canjear.
        /// Este se ejecuta al dar clic en el botón "Vaciar carrito" en la vista del carrito.
        /// </summary>
        public void Clear()
        {
            _items.Clear();
            PointsToRedeem = 0;
            NotifyStateChanged();
        }

        /// <summary>
        /// Obtiene el subtotal del carrito sin aplicar descuentos.
        /// </summary>
        /// <returns></returns>
        public decimal GetSubTotal()
        {
            return _items.Sum(i => i.Subtotal);
        }

        /// <summary>
        /// Obtiene el descuento aplicado por los puntos canjeados.
        /// </summary>
        /// <returns></returns>
        public decimal GetDiscount()
        {
            const decimal pointsValue = 1; // 1 puntos = $1 de descuento (1 punto->1dolar)
            return PointsToRedeem / pointsValue;
        }

        /// <summary>
        /// Obtiene el total del carrito aplicando el descuento de puntos.
        /// </summary>
        /// <returns></returns>
        public decimal GetTotal()
        {
            return Math.Max(GetSubTotal() - GetDiscount(), 0);
        }

        /// <summary>
        /// Obtiene el total de puntos ganados por los productos en el carrito.
        /// </summary>
        /// <returns></returns>
        public int GetTotalEarnedPoints()
        {
            return _items.Sum(i => i.EarnedPoints);
        }

        /// <summary>
        /// Notifica a los suscriptores que el estado del carrito ha cambiado. 
        /// Para que Blazor pueda actualizar la UI automáticamente.
        /// </summary>
        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
    }
}
