using Application.Interfaces;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SaleService : ISaleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SaleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Metodo para agregar una nueva venta.
        /// Usa el UnitOfWork para manejar transacciones y actualizar el inventario de productos y puntos de lealtad del cliente.
        /// </summary>
        /// <param name="sale">Venta a crear</param>
        /// <returns>Id de la venta creada</returns>
        public async Task<int> AddAsync(Ventas sale)
        {
            // Iniciar una transacción
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Agregar la venta al repositorio
                await _unitOfWork.Ventas.AddAsync(sale);

                // Actualizar los puntos de lealtad del cliente
                var cliente = await _unitOfWork.Clientes.GetByIdAsync(sale.ClientId);
                if (cliente != null)
                {
                    cliente.LoyaltyPoints += sale.PointsEarned - sale.PointsUsed;
                    await _unitOfWork.Clientes.UpdateAsync(cliente);
                }

                // Actualizar el inventario de productos....



                // Guardar los cambios
                await _unitOfWork.SaveChangesAsync();

                // Obtener el ID de la venta recién creada
                var result = sale.Id;

                // Confirmar la transacción
                await _unitOfWork.CommitAsync();

                return result;
            }
            catch (Exception)
            {
                // Si ocurre un error, revertir la transacción
                await _unitOfWork.RollbackAsync();
                throw; // Re-lanzar la excepción para manejarla en otro lugar si es necesario
            }
        }

        /// <summary>
        /// Metodo para obtener una venta por su ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Ventas?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Ventas.GetByIdAsync(id);
        }

        /// <summary>
        /// Metodo para obtener todas las ventas de un usuario por su ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Ventas>> GetByUserIdAsync(int userId) 
        {
            return await _unitOfWork.Ventas.GetByUserIdAsync(userId);
        }

        public async Task<List<string>> GetPayMethods()
        {             // Simulación de carga de métodos de pago
            return await Task.FromResult(new List<string>
            {
                "Cash",
                "CreditCard",
                "DebitCard",
                "BankTransfer",
                "Other"
            });
        }

    }
}
