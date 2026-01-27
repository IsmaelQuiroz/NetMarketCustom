using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public  class Cliente : ClaseBase
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string email { get; set; }
        public string Telefono { get; set; }
        public string Imagen {get ; set; }


    }
}
