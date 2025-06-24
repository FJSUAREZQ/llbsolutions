using Domain.RepositoryInterfaces;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly AppDbContext _appDbContext;
        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task<List<Usuarios>> GetAllAsync()
        {
            return await _appDbContext.Usuarios.AsNoTracking().ToListAsync();
        }


        public async Task<Usuarios?> GetByIdAsync(int id)
        {
            return await _appDbContext.Usuarios.FindAsync(id);
        }

        public async Task<Usuarios?> GetByUserNameAsync(string userName)
        {
            return await _appDbContext.Usuarios.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == userName);
        }


        public async Task AddAsync(Usuarios entity)
        {
            await _appDbContext.Usuarios.AddAsync(entity);
            //await _appDbContext.SaveChangesAsync();
        }


        public async Task UpdateAsync(Usuarios entity)
        {
            _appDbContext.Usuarios.Update(entity);
            //_appDbContext.SaveChanges();
        }


        public async Task DeleteAsync(Usuarios entity)
        {
            _appDbContext.Usuarios.Remove(entity);
            //_appDbContext.SaveChanges();
        }


    }
}
