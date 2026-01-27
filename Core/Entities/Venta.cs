using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Venta : ClaseBase
    {
        //[DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
        //[Column(TypeName ="decimal(18,4)")]
        public decimal Monto { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set;}
        //public int CategoriaProductoId { get; set; }
        //public CategoriaProducto CategoriaProducto { get; set;}
        public bool cancelada { get; set; }
        
    }
}
