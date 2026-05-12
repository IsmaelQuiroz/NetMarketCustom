using Core.Entities.OrdenCompra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrdenCompra
{
    public class OrdenCompras : ClaseBase
    {
        public OrdenCompras()
        {
        }

        public OrdenCompras(string compradorEmail, Core.Entities.OrdenCompra.Direccion direccionEnvio, TipoEnvio tipoEnvio, IReadOnlyList<OrdenItem> orderItems, decimal subtotal)
        {
            CompradorEmail = compradorEmail;
            DireccionEnvio = direccionEnvio;
            TipoEnvio = tipoEnvio;
            OrderItems = orderItems;
            Subtotal = subtotal;
        }

        public string CompradorEmail { get; set; }
        public DateTimeOffset OrdenCompra { get; set; } = DateTimeOffset.Now;
        public Core.Entities.OrdenCompra.Direccion DireccionEnvio { get; set; }
        public TipoEnvio TipoEnvio { get; set; }
        public IReadOnlyList<OrdenItem> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public OrdenStatus Status { get; set; } = OrdenStatus.Pendiente;
        public string PagoIntentoId { get; set; }
        public decimal GetTotal () 
        {
            return Subtotal + TipoEnvio.Precio;
        }

    }
}
