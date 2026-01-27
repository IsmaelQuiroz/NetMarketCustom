using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Data
{
    public static class DbcontextExtensions
    {
        public static async Task<bool> HasPendingMigrtionsAsync(this DbContext context)
        {
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            return pendingMigrations.Any();
        }
    }
}
