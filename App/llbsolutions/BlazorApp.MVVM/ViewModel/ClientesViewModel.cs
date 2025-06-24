using Application.Interfaces;
using BlazorApp.MVVM.Components.SharedMVVM.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Shared.DTOs;

namespace BlazorApp.MVVM.ViewModel
{
    public partial class ClientesViewModel : ProtectedViewModelBase
    {
        private readonly ICustomerService _customerService;
        

        public ClientesViewModel(ProtectedLocalStorage storage, NavigationManager nav, ICustomerService customerService)
            : base(storage, nav) 
        { 
            this._customerService = customerService;
        }

        [ObservableProperty]
        public IEnumerable<ClientesDTO> customers = new List<ClientesDTO>();

        [ObservableProperty]
        public string errorMessage= string.Empty;

        [ObservableProperty]
        public bool isLoading =true;

        // cargar los clientes al iniciar la vista
        public async Task LoadCustomersAsync()
        {
            try
            {
                isLoading = true;
                errorMessage = null;

                var _customersDB = await _customerService.GetClientsAsync();

                customers = _customersDB?.Any() == true
                    ? _customersDB.Select(c => new ClientesDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Email = c.Email,
                        LoyaltyPoints = c.LoyaltyPoints
                    })
                    : new List<ClientesDTO>();
            }
            catch (Exception ex)
            {
                errorMessage = "Ocurrió un error al cargar los clientes.";
                Console.WriteLine($"[HomeViewModel] Error: {ex.Message}");
            }
            finally
            {
                isLoading = false;
            }
        }



    }

}
