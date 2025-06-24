using Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using CommunityToolkit.Mvvm.Input;
using Shared.DTOs;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;


namespace BlazorApp.MVVM.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly NavigationManager _navManager;
        private readonly ProtectedLocalStorage _storage;//Para almacenar datos protegidos en el navegador como si el usuario estuviera autenticado


        public LoginDTO LoginDTO { get; set; } = new LoginDTO();

        /// <summary>
        /// Constructor para el ViewModel de Login.
        /// </summary>
        /// <param name="authService"></param>
        /// <param name="nav"></param>
        /// <param name="storage"></param>
        public LoginViewModel(IAuthService authService, NavigationManager nav, ProtectedLocalStorage storage ) 
        {
            this._authService = authService;
            this._navManager = nav;
            this._storage = storage;
        }


        [ObservableProperty]
        private string errorMessage;// Mensaje de error para mostrar en la vista si hay un problema al iniciar sesión.



        [RelayCommand]
        private async Task Login()
        {
            try {
                if (await _authService.AuthenticateAsync(LoginDTO.Username, LoginDTO.Password))
                {
                    errorMessage = string.Empty;

                    await _storage.SetAsync("IsAuthenticated", true);
                    await _storage.SetAsync("UserName", LoginDTO.Username);


                    // Espera breve para sincronizar el circuito interactivo
                    await Task.Delay(100);
                    _navManager.NavigateTo("/ProductosVM", forceLoad:true);
                }
                else
                {
                    ErrorMessage = "Usuario o contraseña incorrectos";
                    await _storage.SetAsync("IsAuthenticated", false);
                    await _storage.SetAsync("UserName", string.Empty);
                }
            }
            catch (Exception ex) 
            {
                errorMessage = "Se presentó un error: " + ex.Message;
                await _storage.SetAsync("IsAuthenticated", false);
                await _storage.SetAsync("UserName", string.Empty);
                await Task.Delay(100);
            }
        }



        [RelayCommand]
        private async Task LogOut()
        {
            try {
                await _authService.LogoutAsync();
                await _storage.SetAsync("IsAuthenticated", false);
                await _storage.SetAsync("UserName", string.Empty);
                _navManager.NavigateTo("/", forceLoad: true);
            }
            catch (Exception ex)
            {
                errorMessage = "Se presentó un error: " + ex.Message;
            }
            finally
            {
                await _authService.LogoutAsync();
                await _storage.SetAsync("IsAuthenticated", false);
                await _storage.SetAsync("UserName", string.Empty);
                _navManager.NavigateTo("/", forceLoad: true);
            }
        }




    }
}
