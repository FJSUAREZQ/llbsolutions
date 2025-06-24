using Application.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BlazorApp.MVVM.Components.SharedMVVM.ViewModels
{

    /// <summary>
    /// Clase base para ViewModels que requieren autenticación.
    /// Se utiliza para proteger las vistas que requieren que el usuario esté autenticado.
    /// Usa IAuthService para verificar el estado de autenticación del usuario.
    /// </summary>
    public abstract class ProtectedViewModelBase: ObservableObject
    {
        private bool _verificacionEjecutada = false;// indica si la verificación de sesión ya se ha ejecutado
        protected readonly ProtectedLocalStorage _storage; // almacena datos protegidos en el navegador, como el estado de autenticación del usuario
        protected readonly NavigationManager _navigation;// maneja la navegación entre páginas en la aplicación Blazor

        protected ProtectedViewModelBase(ProtectedLocalStorage storage, NavigationManager navigation)
        {
            _storage = storage;
            _navigation = navigation;
        }

        /// <summary>
        /// Verifica si la sesión del usuario está activa.
        /// Si la sesión no está activa, redirige al usuario a la página de inicio que es Login.
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        public async Task<bool> VerificarSesionAsync(bool firstRender)
        {
            if (firstRender && !_verificacionEjecutada)
            {
                _verificacionEjecutada = true;
                var result = await _storage.GetAsync<bool>("IsAuthenticated");

                if (!result.Success || !result.Value)
                {
                    _navigation.NavigateTo("/", forceLoad: true); // Redirección si NO está autenticado
                    return false;
                }
            }

            return true;
        }

    }
}
