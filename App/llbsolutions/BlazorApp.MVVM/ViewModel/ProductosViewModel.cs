using Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using Shared.DTOs;
using BlazorApp.MVVM.Components.SharedMVVM.ViewModels;
using CommunityToolkit.Mvvm.Input;
using BlazorApp.MVVM.Services;

namespace BlazorApp.MVVM.ViewModel
{
    public partial class ProductosViewModel : ProtectedViewModelBase
    {

        private readonly IProductService _productService;
        private readonly IShoppingCartService _cartService;


        public ProductosViewModel(ProtectedLocalStorage storage, NavigationManager nav, IProductService productService, 
                                    IShoppingCartService shoppingCartService)
            : base(storage, nav)
        {
            this._productService = productService;
            this._cartService = shoppingCartService;
        }

        [ObservableProperty]
        public IEnumerable<ProductosDTO> productos = new List<ProductosDTO>();

        [ObservableProperty]
        public string errorMessage = string.Empty;

        [ObservableProperty]
        public bool isLoading = true;

        /// <summary>
        /// Carga los productos desde el servicio de productos.
        /// </summary>
        /// <returns></returns>
        public async Task LoadProductsAsync()
        {
            try
            {
                isLoading = true;
                errorMessage = null;

                var _productsDB = await _productService.GetAllAsync();

                productos = _productsDB?.Any() == true
                    ? _productsDB.Select(c => new ProductosDTO
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Description = c.Description,
                        Price = c.Price,
                        Image = c.Image
                    })
                    : new List<ProductosDTO>();
            }
            catch (Exception ex)
            {
                errorMessage = "Ocurrió un error al cargar los productos.";
                Console.WriteLine($"[ProductosViewModel] Error: {ex.Message}");
            }
            finally
            {
                isLoading = false;
            }
        }


        [RelayCommand]
        public void AgregarAlCarrito(ProductosDTO producto)
        {
            try 
            {
                if (producto == null)
                {
                    errorMessage = "Producto no válido.";
                }

                _cartService.AddProduct(producto);
            }
            catch (Exception ex)
            {
                errorMessage = $"Error al agregar el producto al carrito: {ex.Message}";
            }
           
        }



    }
}
