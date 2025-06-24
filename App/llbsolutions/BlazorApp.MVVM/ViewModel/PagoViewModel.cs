using BlazorApp.MVVM.Components.SharedMVVM.ViewModels;
using BlazorApp.MVVM.Models;
using BlazorApp.MVVM.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorApp.MVVM.Helpers;
using Application.Interfaces;
using System.Formats.Asn1;
using Shared.DTOs;
using Shared.Models;
using PaymentMethod = Shared.Models.PaymentMethod;
using Blazor.DownloadFileFast.Interfaces;
using QuestPDF.Fluent;

namespace BlazorApp.MVVM.ViewModel
{
    public partial class PagoViewModel : ProtectedViewModelBase
    {
        private readonly IShoppingCartService _cartService;
        private readonly ICustomerService _customerService;
        private readonly ISaleService _saleService;
        private readonly IBlazorDownloadFileService _downloadService;// servicio para descargar archivos, como PDF de facturas

        public PagoViewModel(ProtectedLocalStorage storage, NavigationManager nav,IShoppingCartService cartService,ICustomerService customerService,
            ISaleService saleService, IBlazorDownloadFileService downloadService)
            : base(storage, nav)
        {
            _cartService = cartService;
            _customerService = customerService;
            _saleService = saleService;
            _downloadService = downloadService;
        }

        [ObservableProperty]
        private string _userName = string.Empty;

        [ObservableProperty]
        private string _customerName = "Nombre del Cliente";

        [ObservableProperty]
        private string _customerAddress = "Sin dirección";

        [ObservableProperty]
        private string _selectedPaymentMethod = "Cash";

        [ObservableProperty]
        private List<CartItem> _items = new();

        [ObservableProperty]
        private decimal _subTotal;

        [ObservableProperty]
        private decimal _discount;

        [ObservableProperty]
        private decimal _total;

        [ObservableProperty]
        private int _pointsEarned;

        [ObservableProperty]
        private int _pointsUsed;

        [ObservableProperty]
        private int _saleId;

        [ObservableProperty]
        private string _errorMessage="";

        public List<string> PaymentMethods;

        public async Task LoadCartAsync()
        {
            try {
                ErrorMessage = "";
                PaymentMethods = await _saleService.GetPayMethods();
                UserName = _cartService.UserName;
                Items = _cartService.Items;
                SubTotal = _cartService.GetSubTotal();
                Discount = _cartService.GetDiscount();
                Total = _cartService.GetTotal();
                PointsEarned = _cartService.GetTotalEarnedPoints();
                PointsUsed = _cartService.PointsToRedeem;

                var _clienteDB = await _customerService.GetClientByUsernameAsync(UserName);
                if (_clienteDB == null)
                {
                    ErrorMessage = "Cliente no encontrado.";
                    return;
                }

                ClientesDTO _clienteDTO = new()
                {
                    Username = _clienteDB.Username,
                    Name = _clienteDB.Name,
                    City = _clienteDB.Address_City,
                    Street = _clienteDB.Address_Street,
                    Suite = _clienteDB.Address_Suite,
                    Email = _clienteDB.Email,
                    LoyaltyPoints = _clienteDB.LoyaltyPoints
                };
                CustomerName = _clienteDTO.Name;
                CustomerAddress = $"{_clienteDTO.City}, {_clienteDTO.Street}, {_clienteDTO.Suite}";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al cargar el carrito: {ex.Message}";
            }
        }

        [RelayCommand]
        public async Task FinalizePurchaseAsync()
        {
            try {
                if (Items.Count == 0)
                {
                    ErrorMessage = "El carrito está vacío.";
                    return;
                }
                if (string.IsNullOrWhiteSpace(SelectedPaymentMethod))
                {
                    ErrorMessage = "Debe seleccionar un método de pago.";
                    return;
                }

                var _venta = new Ventas
                {
                    ClientId = (await _customerService.GetClientByUsernameAsync(UserName))?.Id ?? 0,
                    Date = DateTime.Now,
                    Total = Total,
                    Subtotal = SubTotal,
                    PointsEarned = PointsEarned,
                    PointsUsed = PointsUsed,
                    PaymentMethod = (PaymentMethod)Enum.Parse(typeof(PaymentMethod), SelectedPaymentMethod),
                    Details = Items.Select(item => new VentaDetalle
                    {
                        ProductId = item.Product.Id,
                        Quantity = item.Quantity,
                        UnitPrice = (decimal)item.Product.Price,
                        Discount =  0
                    }).ToList()
                };

                // Guardar la venta
                SaleId = await _saleService.AddAsync(_venta);

                if (SaleId <= 0)
                {
                    ErrorMessage = "Error al procesar la venta.";
                    return;
                }
                
                
                VentaDTO _ventaDTO = new()
                {
                    Id = SaleId,
                    ClientId = _venta.ClientId,
                    ClientName = CustomerName,
                    ClientAddress = CustomerAddress,
                    Date = _venta.Date,
                    Total = _venta.Total,
                    Subtotal = _venta.Subtotal,
                    PointsEarned = _venta.PointsEarned,
                    PointsUsed = _venta.PointsUsed,
                    PaymentMethod = (Shared.DTOs.PaymentMethod)(PaymentMethod)Enum.Parse(typeof(PaymentMethod), _venta.PaymentMethod.ToString()),
                    Details = Items.Select(item => new VentaDetalleDTO
                    {
                        ProductId = item.Product.Id,
                        ProductName = item.Product.Title,
                        Quantity = item.Quantity,
                        UnitPrice = (decimal)item.Product.Price,
                        Discount = 0
                    }).ToList()
                };

                
                // Generar el PDF de la factura
                await DownloadInvoiceAsync(_ventaDTO);

                // Mostrar mensaje de éxito con el ID de la venta
                ErrorMessage = $"Compra finalizada con éxito. ID de venta: {SaleId}";

                // Actualizar el carrito
                _cartService.Clear();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al finalizar la compra: {ex.Message}";
            }
        }

        public async Task DownloadInvoiceAsync(VentaDTO venta)
        {
            try {

                var doc = new InvoicePdfDocument(venta);
                byte[] pdfBytes = doc.GeneratePdf();
                await _downloadService.DownloadFileAsync($"Factura_{venta.Id}.pdf", pdfBytes, "application/pdf");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al descargar la factura: {ex.Message}";
            }
        }
    }
}
