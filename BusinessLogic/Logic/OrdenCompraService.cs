using Core.Entities;
using Core.Entities.OrdenCompra;
//using Core.Entities.OrdenCompras;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Logic
{
    public class OrdenCompraService : IOrdenCompraService
    {
        private readonly IGenericRepository<OrdenCompras> _ordenComprasRepository;
        private readonly IGenericRepository<Producto> _productoRepository;
        private readonly ICarritoCompraRepository _carritoComprasRepository;
        private readonly IGenericRepository<TipoEnvio> _tipoEnvioRepository;

        public OrdenCompraService(IGenericRepository<OrdenCompras> ordenComprasRepository, IGenericRepository<Producto> productoRepository, ICarritoCompraRepository carritoComprasRepository, IGenericRepository<TipoEnvio> tipoEnvioRepository)
        {
            _ordenComprasRepository = ordenComprasRepository;
            _productoRepository = productoRepository;
            _carritoComprasRepository = carritoComprasRepository;
            _tipoEnvioRepository = tipoEnvioRepository;
        }

        public async Task<OrdenCompras> AddOrdenCompraAsync(string compradorEmail, int tipoEnvio, string carritoId, Core.Entities.OrdenCompra.Direccion direccion)
        {
            var carritoCompra = await _carritoComprasRepository.GetCarritoCompraAsync(carritoId);
            var items = new List<OrdenItem>();
            foreach (var item in carritoCompra.Items)
            {
                var productoItem = await _productoRepository.GetByIdAsync(item.Id);
                //se crea un item basado en su constructor de clase
                var itemOrdenado = new ProductoItemOrdenado(productoItem.Id, productoItem.Nombre, productoItem.Imagen);
                var ordenItem = new OrdenItem(itemOrdenado, productoItem.Precio, item.Cantidad);
                items.Add(ordenItem);
            }

            var tipoEnvioEntity = await _tipoEnvioRepository.GetByIdAsync(tipoEnvio);

            var subtotal = items.Sum(item => item.Precio * item.Cantidad);
            var ordenCompra = new OrdenCompras(compradorEmail, direccion, tipoEnvioEntity, items, subtotal);
            return ordenCompra;
        }

        public Task<OrdenCompras> GetOrdenComprasByIdAsync(int id, string email)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<OrdenCompras>> GetOrdenComprasByUserEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<TipoEnvio>> GetTipoEnvios()
        {
            throw new NotImplementedException();
        }
    }
}
