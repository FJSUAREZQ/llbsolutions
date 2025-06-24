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
    public class ProductRepository : IProductRepository
    {

        private readonly AppDbContext _appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }


        public async Task<List<Productos>> GetAllAsync()
        {
            return await _appDbContext.Productos.AsNoTracking().ToListAsync();
        }


        public async Task<Productos?> GetByIdAsync(int id)
        {
            return await _appDbContext.Productos.FindAsync(id);
        }


        public async Task AddAsync(Productos entity)
        {
            await _appDbContext.Productos.AddAsync(entity);
            //await _appDbContext.SaveChangesAsync();
        }


        public async Task UpdateAsync(Productos entity)
        {
            _appDbContext.Productos.Update(entity);
            //_appDbContext.SaveChanges();
        }


        public async Task DeleteAsync(Productos entity)
        {
            _appDbContext.Productos.Remove(entity);
            //_appDbContext.SaveChanges();
        }


        

    }
}
