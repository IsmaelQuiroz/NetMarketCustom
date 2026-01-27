using BusinessLogic.Data;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Logic
{
    public class VentaRepository : IVentaRepository
    {
        private readonly MarketDbContext _context;

        public VentaRepository(MarketDbContext context)
        {
            _context = context;
        }

        public async Task<Venta> GetVentaByIdAsync(int id)
        {
            return await _context.Venta.FindAsync(id);
        }

        public async Task<IReadOnlyList<Venta>> GetVentasAsync()
        {
            return await _context.Venta.ToListAsync();
        }
    }
}
