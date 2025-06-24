using Application.Helpers;
using Application.Interfaces;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        public bool IsAuthenticated { get; private set; } = false;
        public string UserName { get; private set; }


        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<List<Usuarios>> GetAllAsync()
        {
            List<Usuarios> _usuarios = await _unitOfWork.Usuarios.GetAllAsync();

            if (_usuarios == null || !_usuarios.Any())
            {
                return new List<Usuarios>();
            }
            return _usuarios;
        }


        public async Task<Usuarios?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Usuarios.GetByIdAsync(id);
        }

        public async Task<Usuarios?> GetByUsernameAsync(string username)
        {
            return await _unitOfWork.Usuarios.GetByUserNameAsync(username);
        }


        public async Task<int> AddAsync(Usuarios entity)
        {
            await _unitOfWork.Usuarios.AddAsync(entity);
            return await _unitOfWork.SaveChangesAsync();
        }


        public async Task<int> UpdateAsync(Usuarios entity)
        {
            await _unitOfWork.Usuarios.UpdateAsync(entity);
           return await _unitOfWork.SaveChangesAsync();
        }


        public async Task<int> DeleteAsync(Usuarios entity)
        {
            await _unitOfWork.Usuarios.DeleteAsync(entity);
            return await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Autentica al usuario con el nombre de usuario y la contraseña proporcionados.
        /// </summary>
        /// <param name="username">Nombre de Usuario</param>
        /// <param name="password">Contraseña</param>
        /// <returns>true si el usuario esta autenticado, false si no</returns>
        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            this.IsAuthenticated = false;
            this.UserName = string.Empty;

            var user = await _unitOfWork.Usuarios.GetByUserNameAsync(username);

            if (user == null)
            {
                return false; // Usuario no encontrado
            }

            // Verificar la contraseña con hashing
            if (HashHelper.VerifyHash(password, user.Password)) {
                this.IsAuthenticated = true;
                this.UserName = username;

                return true; // Autenticación exitosa
            }
            else
            {
                return false; // Contraseña incorrecta
            }
        }

        /// <summary>
        /// Cierra la sesión del usuario actual y limpia el estado de autenticación.
        /// </summary>
        /// <returns></returns>
        public async Task LogoutAsync()
        {
            IsAuthenticated = false; // Limpiar el estado de autenticación
            UserName = string.Empty; // Limpiar el nombre de usuario
        }




    }
}
