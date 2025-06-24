using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICustomerService
    {
        Task<List<Clientes>> GetClientsAsync();
        Task<Clientes?> GetClientAsync(int id);
        Task<Clientes?> GetClientByUsernameAsync(string username);
        Task<int> AddClientAsync(Clientes client);
        Task<int> UpdateClientAsync(Clientes client);
        Task<int> DeleteClientAsync(int id);
    }
}
