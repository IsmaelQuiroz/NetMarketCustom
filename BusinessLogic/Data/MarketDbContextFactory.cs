using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Data
{
    public class MarketDbContextFactory : IDesignTimeDbContextFactory<MarketDbContext>
    {
        public MarketDbContext CreateDbContext(string[] args)
        {
            //throw new NotImplementedException();
            
            //Confguracion 1 -- Probado que si funciono
            //IConfigurationRoot configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json")
            //.Build();

            //var builder = new DbContextOptionsBuilder<MarketDbContext>();
            //var connectionString = configuration.GetConnectionString("DefaultConnection");

            //builder.UseSqlServer(connectionString);

            //return new MarketDbContext(builder.Options);


            //Configuracion 2

            var optionsBuilder = new DbContextOptionsBuilder<MarketDbContext>();
            optionsBuilder.UseSqlServer("DefaultConnection");
            return new MarketDbContext(optionsBuilder.Options);

        }
    }
}
