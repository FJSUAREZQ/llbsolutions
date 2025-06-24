using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISaleService
    {
        Task<int> AddAsync(Ventas sale);
        Task<Ventas?> GetByIdAsync(int id);
        Task<List<Ventas>> GetByUserIdAsync(int userId);
        Task<List<string>> GetPayMethods();
    }
}
