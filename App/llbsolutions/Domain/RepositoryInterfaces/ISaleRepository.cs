using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface ISaleRepository
    {
        Task<List<Ventas>> GetAllAsync();
        Task AddAsync(Ventas sale);
        Task<Ventas?> GetByIdAsync(int id);
        Task<List<Ventas>> GetByUserIdAsync(int userId);
        Task UpdateAsync(Ventas sale);
        Task DeleteAsync(Ventas sale);

    }
}
