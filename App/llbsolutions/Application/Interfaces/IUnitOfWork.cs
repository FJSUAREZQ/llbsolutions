using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.RepositoryInterfaces;

namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        // Definición de los repositorios
        ICustomerRepository Clientes { get; }
        IProductRepository Productos { get; }
        ISaleRepository Ventas { get; }
        IUserRepository Usuarios { get; }



        // Métodos de transacción
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();


        // Guardar cambios
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
