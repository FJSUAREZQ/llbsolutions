using Application.Interfaces;
using BlazorApp.MVVM.Components.SharedMVVM.ViewModels;
using BlazorApp.MVVM.Models;
using BlazorApp.MVVM.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;

namespace BlazorApp.MVVM.ViewModel
{
    public partial class CarritoViewModel : ProtectedViewModelBase
    {
        private readonly IShoppingCartService _cartService;
        private readonly ICustomerService _customerService;

        public CarritoViewModel(ProtectedLocalStorage storage, NavigationManager nav,ICustomerService customerService,IShoppingCartService shoppingCartService)
            : base(storage, nav)
        {
            _cartService = shoppingCartService;
            this._customerService = customerService;
        }

        [ObservableProperty]
        private ObservableCollection<CartItem> _items = new();

        [ObservableProperty]
        private decimal _subTotal;

        [ObservableProperty]
        private decimal _discount;

        [ObservableProperty]
        private decimal _total;

        [ObservableProperty]
        private int _totalEarnedPoints;

        [ObservableProperty]
        private int _pointsToRedeem;

        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private string _errorMessage;

        public int AvailablePoints => _cartService.AvailablePoints;

        /// <summary>
        /// Carga el carrito de compras desde el servicio de carrito.
        /// </summary>
        /// <returns></returns>
        public async Task LoadCartAsync()
        {
            try {
                var userName = await _storage.GetAsync<string>("UserName");
                UserName = userName.Success ? userName.Value : string.Empty; // Manejo de nombre de usuario
                _cartService.UserName = UserName; // Asignar nombre de usuario al servicio del carrito

                var customer = await _customerService.GetClientByUsernameAsync(userName.Value.ToString());
                if (customer != null)
                {
                    _cartService.AvailablePoints = customer?.LoyaltyPoints ?? 0; // Asignar puntos disponibles del cliente
                }


                Items = new ObservableCollection<CartItem>(_cartService.Items);
                PointsToRedeem = _cartService.PointsToRedeem;
                CalculateTotals();
            }
            catch (Exception ex) 
            {
                ErrorMessage = $"Error al cargar el carrito: {ex.Message}";
            }
        }

        /// <summary>
        /// Calcula los totales del carrito de compras: subtotal, descuento, total y puntos ganados.
        /// </summary>
        private void CalculateTotals()
        {
            try {
                SubTotal = _cartService.GetSubTotal();
                Discount = _cartService.GetDiscount();
                Total = _cartService.GetTotal();
                TotalEarnedPoints = _cartService.GetTotalEarnedPoints();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al calcular totales: {ex.Message}";
            }
        }

        /// <summary>
        /// Actualiza la cantidad de un producto en el carrito de compras.
        /// </summary>
        /// <param name="item"></param>
        [RelayCommand]
        public void UpdateQuantity(CartItem item)
        {
            try {
                if (item.Quantity < 1) 
                    item.Quantity = 1;

                _cartService.UpdateQuantity(item.Product.Id, item.Quantity);
                CalculateTotals();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al actualizar la cantidad: {ex.Message}";
            }
        }

        /// <summary>
        /// Elimina un producto del carrito de compras.
        /// </summary>
        /// <param name="item"></param>
        [RelayCommand]
        public void RemoveItem(CartItem item)
        {
            try {
                _cartService.RemoveProduct(item.Product.Id);
                Items.Remove(item); // optimización visual
                CalculateTotals();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al eliminar el producto: {ex.Message}"; // Manejo de errores
            }
        }

        /// <summary>
        /// Navega a la página de pago para proceder con la compra.
        /// </summary>
        [RelayCommand]
        public void BuyNow()
        {
            try {
                if (Items.Count == 0)
                {
                    ErrorMessage = "El carrito está vacío. No se puede realizar la compra.";
                    return;
                }

                ErrorMessage = string.Empty;
                _navigation.NavigateTo("/PagoVM");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al proceder con la compra: {ex.Message}"; // Manejo de errores
            }
        }

        /// <summary>
        /// Aplica los puntos de fidelización al carrito de compras.
        /// Valida que los puntos a canjear no sean negativos, no excedan los puntos disponibles y no superen el 10% del total del carrito.
        /// </summary>
        [RelayCommand]
        public void ApplyPoints()
        {
            try {
                var maxPointsToUse = (int)((Total * 10) / 100); //el porcentaje de puntos a canjear es 10% del total
                if (maxPointsToUse <= 0)
                {
                    ErrorMessage = "No se puede canjear puntos, el total es cero.";
                    return;
                }
                if (PointsToRedeem < 0)
                {
                    ErrorMessage = "Los puntos a canjear no pueden ser negativos.";
                    return;
                }
                if (PointsToRedeem > AvailablePoints)
                {
                    ErrorMessage = "No tienes suficientes puntos disponibles para canjear.";
                    return;
                }
                if (PointsToRedeem > maxPointsToUse)
                {
                    ErrorMessage = $"No puedes canjear más de {maxPointsToUse} puntos por este pedido.";
                    return;
                }

                ErrorMessage = string.Empty; // Limpiar mensaje de error si todo está bien

                var pointsToUse = Math.Min(PointsToRedeem, AvailablePoints);
                pointsToUse = Math.Min(pointsToUse, maxPointsToUse);

                _cartService.PointsToRedeem = pointsToUse;
                PointsToRedeem = pointsToUse;

                CalculateTotals();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al aplicar puntos: {ex.Message}"; // Manejo de errores
            }
        }

        /// <summary>
        /// Limpia el carrito de compras, eliminando todos los productos.
        /// </summary>
        [RelayCommand]
        public void ClearCart()
        {
            try
            {
                _cartService.Clear();
                Items.Clear();
                CalculateTotals();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al limpiar el carrito: {ex.Message}"; // Manejo de errores
            }
        }
    }
}