using Core.Entities;
using Core.Entities.OrdenCompra;
//using Core.Entities.OrdenCompras;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Data
{
    public class MarketDbContext : DbContext
    {
        public MarketDbContext(DbContextOptions<MarketDbContext> options) : base(options) { }

        public DbSet<Cliente> Cliente { get; set; } 
        public DbSet<Venta> Venta { get; set; }
        public DbSet<CategoriaProducto> CategoriaProducto { get; set; }
        public DbSet<Producto> Producto {  get; set; }  
        public DbSet<Marca> Marca { get; set; }

        public DbSet<OrdenCompras> OrdenCompras { get; set;}
        public DbSet<OrdenItem> OrdenItems {  get; set; }
        public DbSet<TipoEnvio> TipoEnvios { get; set; }

        //para Aplicar las configuraciones de BusinessLogic.Data.Configuration para cada propiedad de las Entities
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
