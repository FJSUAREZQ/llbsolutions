using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<List<Productos>> GetAllAsync();
        Task<Productos?> GetByIdAsync(int id);
        Task<int> AddAsync(Productos entity);
        Task<int> Update(Productos entity);
        Task<int> Delete(Productos entity);
    }
}
