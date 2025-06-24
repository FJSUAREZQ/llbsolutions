using Application.Interfaces;
using Domain.RepositoryInterfaces;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork, IAsyncDisposable
    {

        private readonly AppDbContext _context;// contexto de la base de datos que se utilizará para las operaciones de UoW
        private bool _disposed;// indica si el objeto ya ha sido liberado
        private IDbContextTransaction _transaction;

        public IUserRepository Usuarios { get; private set; }
        public ICustomerRepository Clientes { get; private set; }
        public IProductRepository Productos { get; private set; }
        public ISaleRepository Ventas { get; private set; }



        public UnitOfWork(
                            AppDbContext context,
                            IUserRepository usuarios,
                            ICustomerRepository clientes,
                            IProductRepository productos,
                            ISaleRepository ventas )
        {
            _context = context;
            Usuarios = usuarios;
            Clientes = clientes;
            Productos = productos;
            Ventas = ventas;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }


        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            try
            {
                return await _context.SaveChangesAsync(ct);//retorna el número de entidades afectadas por la operación de guardado en la base de datos
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Error al guardar cambios en la base de datos.");
                throw;
            }
        }

        public async Task CommitAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException("No hay una transacción activa.");

            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException("No hay una transacción activa.");

            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                if (_transaction != null)
                    await _transaction.DisposeAsync();

                _context.Dispose();
                _disposed = true;
            }

            GC.SuppressFinalize(this);
        }



    }
}
