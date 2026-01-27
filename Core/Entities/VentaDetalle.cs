using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    internal class VentaDetalle: ClaseBase
    {
        public int ProductoId {  get; set; }
        public decimal Cantidad { get; set; }
        public decimal CUnitario { get; set; }
        public decimal PVenta { get; set; }
        public decimal Importe { get; set;}
        public string Concepto { get; set; }


    }
}
