using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    internal class Compras
    {
        public DateTime FechaCompra { get; set; }
        public int ProvedorId { get; set; } 
        public Provedor Provedor { get; set; }
        public int Cantidad { get; set; }
        public decimal CUnitario { get; set; }  
        public int ProductoId { get; set; } 
        public Producto Producto { get; set; }  


    }
}
