using BusinessLogic.Data;
using BusinessLogic.Logic;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Net.Security;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Security.Policy;
using System.Text;
using WebApi.Dtos;
using WebApi.Middleware;

namespace WebApi;

public class Startup
{   
    public Startup (IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        //Token #
        services.AddScoped<ITokenService, TokenService>();

        /*Seguridad: 8 Inyectar el Servicio de IdentityCore al interior de nuestro proyecto WebApi,
        para que se ejecute el proceso de Migration o CodeFirst ya para la creación de las tablas en SQL
        este objeto es la instancia del EntityCore , la representación del modelo */
        var builder = services.AddIdentityCore<Usuario>();
        //Agregarle los servicios para el userType
        builder = new IdentityBuilder(builder.UserType, builder.Services); //esto es lo que necesita el objeto para poder construir las tablas desde el modelo del IdentityCore
        builder.AddEntityFrameworkStores<SeguridadDbContext>();
        builder.AddSignInManager<SignInManager<Usuario>>(); //

        //Seguridad: 9 Indicarle que magregue el manejo de la autenticación
        //Token #: Se configura la seguridad de la Aplicacion en base al Token
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:Key"])),
                ValidIssuer = Configuration["Token:Issuer"], //Valor del servidor que esta generando el token
                ValidateIssuer = true,
                ValidateAudience = false
            };
        });

        //Token #: Se agregan los Token:Key y Token:Issuer al appsettings.json

        services.AddAutoMapper(typeof(MappingProfiles));

        services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
        services.AddScoped(typeof(IGenericSeguridadRepository<>), (typeof(GenericSeguridadRepository<>))); //Generic Repository Pattern 5

        //services.AddDbContext<MarketDbContext>();
        services.AddDbContext<MarketDbContext>(opt => {
            opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        });

        //Seguridad:3
        services.AddDbContext<SeguridadDbContext>(x =>
        {
            x.UseSqlServer(Configuration.GetConnectionString("IdentitySeguridad"));
        });

        //Seguridad:4, en el appsettings.json definir la cadena de conexión  "IdentitySeguridad"
        //Seguridad:5, ir SSMS de SQL Server y crear manualmente la Base de Datos IdentitySeguridad, 
        //Seguridad:6 Ejecutar el comando dotnet... para agregar los archivos de Migración de SeguridadDbContext

        //Redis1:Singleton significa que el programa va crear una sola instancia de objeto conexión para el Redis
        //que va ser utilizado durante todo el ciclo de vida del programa
        services.AddSingleton<IConnectionMultiplexer>( c =>
        {
            var configuration = ConfigurationOptions.Parse(Configuration.GetConnectionString("Redis"), true);
            return ConnectionMultiplexer.Connect(configuration);

        });

        //Redis2: Agregar la propiedad redis al interior del appsettings.json

        services.AddTransient<IClienteRepository, ClienteRepository>();
        services.AddTransient<IVentaRepository, VentaRepository>();
        services.AddTransient<IProductoRepository, ProductoRepository>();
        services.AddControllers();

        //Redis5: servicios para Carritocompras para que cuando se arranque el proyecto se inicialicen las operaciones
        //de la interface con su implementación
        //se hace la inyección y esto posibilita utilizar le interface en cualquier clase del proyecto
        services.AddScoped<ICarritoCompraRepository, CarritoCompraRepository>();

        //Adicionado
        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsRule", rule =>
            {
                rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        //if (env.IsDevelopment())
        //{
        //    app.UseDeveloperExceptionPage();
        //}


        //agrego el Middleware que acabo de crear
        app.UseMiddleware<ExceptionMiddleware>();

        //cuando sale un error como este en React--browser
        //Access to XMLHttpRequest at 'http://localhost:5000/api/producto'
        //from origin 'http://localhost:3000' has been blocked by CORS policy:
        //No 'Access-Control-Allow-Origin' header is present on the requested resource.
        //SI FUNCIONA
        //app.UseCors(options =>
        //{
        //    options
        //    .AllowAnyOrigin()
        //    .AllowAnyHeader()
        //    .AllowAnyMethod();
        //});


        app.UseStatusCodePagesWithReExecute("/errors", "?code={0}");

        app.UseRouting();

        //Cors agregado by turorial
        app.UseCors("CorsRule");

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

