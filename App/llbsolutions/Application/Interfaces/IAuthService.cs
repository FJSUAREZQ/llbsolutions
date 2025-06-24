using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<List<Usuarios>> GetAllAsync();
        Task<Usuarios?> GetByIdAsync(int id);
        Task<Usuarios?> GetByUsernameAsync(string username);
        Task<int> AddAsync(Usuarios entity);
       // Task<int> UpdateAsync(Usuarios entity);
        //Task<int> DeleteAsync(Usuarios entity);

        Task<bool> AuthenticateAsync(string username, string password);
        bool IsAuthenticated { get; }
        string UserName { get; }
        Task LogoutAsync();
    }

 }
