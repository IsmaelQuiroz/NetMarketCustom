using BusinessLogic.Data;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Logic
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly MarketDbContext _context;

        public ProductoRepository(MarketDbContext context)
        {
           _context = context;
        }

        public async Task<Producto> GetProductoByIdAsync(int id)
        {
            //return await _context.Producto.FindAsync(id);
            return await _context.Producto
                .Include(p => p.categoria)
                .Include(p => p.marca)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<Producto>> GetProductosAsync()
        {
            //return await _context.Producto.ToListAsync();
            return await _context.Producto
                .Include(p => p.categoria)
                .Include( p=> p.marca)
                .ToListAsync();
        }
    }
}
