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
    public class SaleRepository :ISaleRepository
    {
        private readonly AppDbContext _appDbContext;
        public SaleRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Ventas>> GetAllAsync()
        {
            return await _appDbContext.Ventas.AsNoTracking().ToListAsync();
        }


        public async Task<Ventas?> GetByIdAsync(int id)
        {
            return await _appDbContext.Ventas
                .Include(s => s.Details)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Ventas>> GetByUserIdAsync(int userId)
        {
            return await _appDbContext.Ventas
                .Include(s => s.Details)
                .Where(s => s.ClientId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Ventas entity)
        {
            await _appDbContext.Ventas.AddAsync(entity);
            //await _appDbContext.SaveChangesAsync();
        }


        public async Task UpdateAsync(Ventas entity)
        {
            _appDbContext.Ventas.Update(entity);
            //_appDbContext.SaveChanges();
        }


        public async Task DeleteAsync(Ventas entity)
        {
            _appDbContext.Ventas.Remove(entity);
            //_appDbContext.SaveChanges();
        }

       

    }
}
