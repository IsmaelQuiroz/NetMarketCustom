using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Producto : ClaseBase
    {   
        public string Modelo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Costo { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int MinStock { get; set; }
        public int MaxStock { get; set; }
        public int CategoriaId { get; set; }
        public CategoriaProducto categoria {  get; set; }
        public int MarcaId { get; set; }
        public Marca marca { get; set; }    
        public string Atributo { get; set; }
        public string OwnerT { get; set; }
        public string Ubicacion { get; set; }  

    }
}
