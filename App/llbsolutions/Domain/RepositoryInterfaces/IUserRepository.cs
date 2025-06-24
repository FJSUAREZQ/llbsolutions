using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<List<Usuarios>> GetAllAsync();
        Task<Usuarios?> GetByIdAsync(int id);
        Task<Usuarios> GetByUserNameAsync(string userName);
        Task AddAsync(Usuarios entity);
        Task UpdateAsync(Usuarios entity);
        Task DeleteAsync(Usuarios entity);
    }
}
