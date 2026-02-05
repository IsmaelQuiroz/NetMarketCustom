using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductoWithCategoriaAndMarcaSpecification : BaseSpecification<Producto>
    {
        public ProductoWithCategoriaAndMarcaSpecification()
        {
            //Aqui vamos a agregar los Include relaciones que va tener esta entidad Producto
            AddInclude(p => p.categoria);
            AddInclude(p => p.marca);
        }

        //Aqui las condiciones con relaciones
        public ProductoWithCategoriaAndMarcaSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(p => p.categoria);
            AddInclude(p => p.marca);
        }


    }
}
