using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessLogic.Data
{
    //Redis4: clase que implementará a IcarritoCompra
    public class CarritoCompraRepository : ICarritoCompraRepository
    {
        //Redis6: Inyectado de objeto manejador de la base de datos Redis
        private readonly IDatabase _database;

        public CarritoCompraRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<CarritoCompra> GetCarritoCompraAsync(string carritoId)
        {
           var data = await _database.StringGetAsync(carritoId);

            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CarritoCompra>(data);
        }

        //Create or Update
        public async Task<CarritoCompra> UpdateCarritoCompraAsync(CarritoCompra carritoCompra)
        {
           var status = await _database.StringSetAsync(carritoCompra.Id, JsonSerializer.Serialize(carritoCompra), TimeSpan.FromDays(30));
            
            if (!status) return null;

            return await GetCarritoCompraAsync(carritoCompra.Id);
        }

        public async Task<bool> DeleteCarritoCompraAsync(string carritoId)
        {
           return await _database.KeyDeleteAsync(carritoId);
        }        

        
    }
}
