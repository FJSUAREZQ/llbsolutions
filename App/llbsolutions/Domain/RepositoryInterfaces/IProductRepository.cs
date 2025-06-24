using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IProductRepository
    {
        Task<List<Productos>> GetAllAsync();
        Task<Productos?> GetByIdAsync(int id);
        Task AddAsync(Productos entity);
        Task UpdateAsync(Productos entity);
        Task DeleteAsync(Productos entity);
    }
}
