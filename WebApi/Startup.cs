using BusinessLogic.Data;
using BusinessLogic.Logic;
using Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Sockets;
using WebApi.Dtos;
using WebApi.Middleware;

namespace WebApi;

public class Startup
{
    public IConfiguration Configuration { get;  }

    public Startup (IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfiles));
        //services.AddDbContext<MarketDbContext>();
        services.AddDbContext<MarketDbContext>(opt => {
            opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddTransient<IClienteRepository, ClienteRepository>();
        services.AddTransient<IVentaRepository, VentaRepository>();
        services.AddTransient<IProductoRepository, ProductoRepository>();
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        //if (env.IsDevelopment())
        //{
        //    app.UseDeveloperExceptionPage();
        //}

        //agrego el Middleware que acabo de crear
        app.UseMiddleware<ExceptionMiddleware>();


        app.UseStatusCodePagesWithReExecute("/errors", "?code={0}");

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

