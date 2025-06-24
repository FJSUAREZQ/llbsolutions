using Application.Interfaces;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Clientes>> GetClientsAsync()
        {
            List<Clientes> _clientes = await _unitOfWork.Clientes.GetAllAsync();

            if (_clientes == null || !_clientes.Any())
            {
                return new List<Clientes>();
            }
            return _clientes;
        }


        public async Task<Clientes?> GetClientAsync(int id)
        {
            return await _unitOfWork.Clientes.GetByIdAsync(id);
        }

        public async Task<Clientes?> GetClientByUsernameAsync(string username)
        {
            return await _unitOfWork.Clientes.GetByUsernameAsync(username);
        }


        public async Task<int> AddClientAsync(Clientes client)
        {
            await _unitOfWork.Clientes.AddAsync(client);

            int result = await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<int> UpdateClientAsync(Clientes client)
        {
            await _unitOfWork.Clientes.UpdateAsync(client);

            int result = await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<int> DeleteClientAsync(int id)
        {
            var client = await _unitOfWork.Clientes.GetByIdAsync(id);
            int result = 0;

            if (client != null)
            {
                await _unitOfWork.Clientes.DeleteAsync(client);
                result = await _unitOfWork.SaveChangesAsync();
            }

            return result;
        }
    }
}
