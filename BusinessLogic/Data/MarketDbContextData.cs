using Core.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BusinessLogic.Data
{
    public class MarketDbContextData
    {
        public static async Task CargaDataAsync(MarketDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Cliente.Any()) {
                    var clienteData = File.ReadAllText("../BusinessLogic/CargarData/cliente.json");
                    var clientes = JsonSerializer.Deserialize<List<Cliente>>(clienteData);

                    foreach(var cl in clientes)
                    {
                        context.Cliente.Add(cl);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.Marca.Any())
                {
                    var marcaData = File.ReadAllText("../BusinessLogic/CargarData/marca.json");
                    var marcas = JsonSerializer.Deserialize<List<Marca>>(marcaData);
                    foreach (var m in marcas)
                    {
                        context.Marca.Add(m);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.CategoriaProducto.Any())
                {
                    var categoriaProductoData = File.ReadAllText("../BusinessLogic/CargarData/categoriaProducto.json");
                    var categorias = JsonSerializer.Deserialize<List<CategoriaProducto>>(categoriaProductoData);
                    foreach(var c in categorias)
                    {
                        context.CategoriaProducto.Add(c);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.Producto.Any())
                {
                    var productoData = File.ReadAllText("../BusinessLogic/CargarData/producto.json");
                    var productos = JsonSerializer.Deserialize<List<Producto>>(productoData);
                    foreach(var p in productos)
                    {
                        context.Producto.Add(p);
                    }
                    await context.SaveChangesAsync();
                }

            }catch (Exception e) { 
                var logger = loggerFactory.CreateLogger<MarketDbContextData>();
                logger.LogError(e.Message);
            }
        }
    }
}
