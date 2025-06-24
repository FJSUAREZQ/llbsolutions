using Application.Helpers;
using Application.Interfaces;
using Shared.DTOs.ApiProductResponse;
using Shared.DTOs.ApiResponse;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SyncService : ISyncService
    {
        private readonly HttpClient _httpClient;
        private readonly IUnitOfWork _unitOfWork;

        public SyncService(HttpClient httpClient, IUnitOfWork unitOfWork)
        {
            _httpClient = httpClient;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Sincronizar clientes desde una API externa a la base de datos local.
        /// </summary>
        /// <returns></returns>
        public async Task SyncClientsAsync()
        {
            var apiClients = await _httpClient.GetFromJsonAsync<List<API_CustomersDto>>("https://jsonplaceholder.typicode.com/users");

            if (apiClients != null)
            {
                foreach (var apiClient in apiClients)
                {
                    var existing = await _unitOfWork.Clientes.GetByIdAsync(apiClient.Id);
                    if (existing == null)
                    {
                        var client = new Clientes
                        {
                            Id = apiClient.Id,
                            Name = apiClient.Name,
                            Username = apiClient.Username,
                            Address_City = apiClient.Address.City,
                            Address_Street = apiClient.Address.Street,
                            Address_Suite = apiClient.Address.Suite,
                            Email = apiClient.Email,
                            LoyaltyPoints = new Random().Next(0, 1001),
                            CreatedDate = DateTime.UtcNow 
                        };
                        await _unitOfWork.Clientes.AddAsync(client);
                    }
                }
                await _unitOfWork.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Sincronizar productos desde una API externa a la base de datos local.
        /// </summary>
        /// <returns></returns>
        public async Task SyncProductsAsync()
        {
            var apiProducts = await _httpClient.GetFromJsonAsync<List<API_ProductsDto>>("https://fakestoreapi.com/products");

            if (apiProducts != null)
            {
                foreach (var apiProduct in apiProducts)
                {
                    var existing = await _unitOfWork.Productos.GetByIdAsync(apiProduct.Id);
                    if (existing == null)
                    {
                        var product = new Productos
                        {
                            Id = apiProduct.Id,
                            Title = apiProduct.Title,
                            Description = apiProduct.Description,
                            Image = apiProduct.Image,
                            Price = apiProduct.Price,
                            Category = apiProduct.Category
                        };
                        await _unitOfWork.Productos.AddAsync(product);
                    }
                }
                await _unitOfWork.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Permite setear las contraseñas de los usuarios en la base de datos local
        /// Ya que este dato no existe en la API de clientes.
        /// </summary>
        /// <returns></returns>
        public async Task SyncUsersAsync() 
        {
            var users = await _unitOfWork.Clientes.GetAllAsync();

            if (users != null && users.Any())
            {
                foreach (var item in users)
                {
                    var existing = await _unitOfWork.Usuarios.GetByIdAsync(item.Id);
                    if (existing == null)
                    {
                        var usuario = new Usuarios
                        {
                            Id = item.Id,
                            Username = item.Username,
                            Password = HashHelper.HashPassword(item.Email),
                            CreatedDate = DateTime.UtcNow
                        };
                        await _unitOfWork.Usuarios.AddAsync(usuario);
                    }
                }
                await _unitOfWork.SaveChangesAsync();
            }

        }







    }
}
