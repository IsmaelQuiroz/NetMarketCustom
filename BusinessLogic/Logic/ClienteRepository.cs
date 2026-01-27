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
    public class ClienteRepository : IClienteRepository
    {
        private readonly MarketDbContext _context;

        public ClienteRepository(MarketDbContext context)
        {
            _context = context;
        }

        public async Task<Cliente> GetClienteByIdAsync(int id)
        {
           return await _context.Cliente.FindAsync(id);
        }

        public async Task<IReadOnlyList<Cliente>> GetClientesAsync()
        {
            return await _context.Cliente.ToListAsync();
        }
    }
}
