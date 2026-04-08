using BusinessLogic.Data;
using Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Net.Security;
using System.Threading.Tasks;

namespace WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        //CreateHostBuilder(args).Build().Run();
        var host = CreateHostBuilder(args).Build();

        //to check Migrations
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var context = services.GetRequiredService<MarketDbContext>();
                //Seecion de prueba checar si hay migraciones pendientes
                if (await context.HasPendingMigrtionsAsync())
                {
                    Console.WriteLine("Existen migraciones pendientes. Aplicándolas...");
                    await context.Database.MigrateAsync();
                    Console.WriteLine("Migraciones aplicadas.");
                }
                else
                {
                    Console.WriteLine("La base de datos está sincronizada con el modelo.");
                }
                await MarketDbContextData.CargaDataAsync(context, loggerFactory);

                //Seguridad 10: Aplicar la Migración relacionada a Seguridad
                var identityContext = services.GetRequiredService<SeguridadDbContext>(); //representa la instancia del Dbcontext para esta parte de Seguridad
                await identityContext.Database.MigrateAsync(); //aqui se ejecuta el proceso de Migración
              
                var userManager = services.GetRequiredService<UserManager<Usuario>>();  //Una vez que la migración concluya, crear el nuevo usuario 

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>(); //Roles 4

                await SeguridadDbContextData.SeedUserAsync(userManager, roleManager);

            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(e, "Errores en el proceso de migracion");
            }
            host.Run();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    => Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
}