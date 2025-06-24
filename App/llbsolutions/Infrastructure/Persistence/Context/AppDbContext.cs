using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Models;

namespace Infrastructure.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }


        public DbSet<Usuarios> Usuarios => Set<Usuarios>();
        public DbSet<Clientes> Clientes => Set<Clientes>();
        public DbSet<Productos> Productos => Set<Productos>();
        public DbSet<Ventas> Ventas => Set<Ventas>();
        public DbSet<VentaDetalle> VentaDetalle => Set<VentaDetalle>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración Fluent API
        }


    }
}
