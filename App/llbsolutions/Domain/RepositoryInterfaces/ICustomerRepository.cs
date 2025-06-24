using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface ICustomerRepository
    {
        Task<List<Clientes>> GetAllAsync();
        Task<Clientes?> GetByIdAsync(int id);
        Task<Clientes?> GetByUsernameAsync(string username);
        Task AddAsync(Clientes entity);
        Task UpdateAsync(Clientes entity);
        Task DeleteAsync(Clientes entity);
    }
}
