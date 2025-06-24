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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext  _appDbContext;

        public CustomerRepository(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext ;
        }


        public async Task<List<Clientes>> GetAllAsync()
        {
            return await _appDbContext.Clientes.AsNoTracking().ToListAsync();
        }


        public async Task<Clientes?> GetByIdAsync(int id)
        {
            return await _appDbContext.Clientes.FindAsync(id);
        }

        public async Task<Clientes?> GetByUsernameAsync(string username)
        {
            return await _appDbContext.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Username == username);
        }


        public async Task AddAsync(Clientes entity)
        {
            await _appDbContext.Clientes.AddAsync(entity);
            //await _appDbContext.SaveChangesAsync();
        }


        public async Task UpdateAsync(Clientes entity)
        {
             _appDbContext.Clientes.Update(entity);
            //_appDbContext.SaveChanges();
        }


        public async Task DeleteAsync(Clientes entity)
        {
            _appDbContext.Clientes.Remove(entity);
            //_appDbContext.SaveChanges();
        }



    }
}
