using Application.Interfaces;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {

        private readonly IUnitOfWork _unitOfWork;

        
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<List<Productos>> GetAllAsync()
        {
            List<Productos> _productos = await _unitOfWork.Productos.GetAllAsync();
            if (_productos == null || !_productos.Any())
            {
                return new List<Productos>();
            }
            return _productos;
        }


        public async Task<Productos?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Productos.GetByIdAsync(id);
        }


        public async Task<int> AddAsync(Productos entity)
        {
            await _unitOfWork.Productos.AddAsync(entity);
            int result = await _unitOfWork.SaveChangesAsync();
            return result;
        }


        public async Task<int> Update(Productos entity)
        {
            await _unitOfWork.Productos.UpdateAsync(entity);
            int result = await _unitOfWork.SaveChangesAsync();
            return result;
        }


        public async Task<int> Delete(Productos entity)
        {
            await _unitOfWork.Productos.DeleteAsync(entity);
            int result = await _unitOfWork.SaveChangesAsync();
            return result;
        }
    }
}
