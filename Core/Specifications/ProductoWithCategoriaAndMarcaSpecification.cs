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
        //public ProductoWithCategoriaAndMarcaSpecification(string sort, int? marca, int? categoria)
        public ProductoWithCategoriaAndMarcaSpecification(ProductoSpecificationParams productoParams)
            : base( x => 
            (string.IsNullOrEmpty(productoParams.Search) || x.Nombre.Contains(productoParams.Search)) &&
            (!productoParams.Marca.HasValue || x.MarcaId == productoParams.Marca) &&
            (!productoParams.Categoria.HasValue || x.CategoriaId == productoParams.Categoria) 
            )
        {
            //Aqui vamos a agregar los Include relaciones que va tener esta entidad Producto
            AddInclude(p => p.categoria);
            AddInclude(p => p.marca);
            //AddOrderBy(p => p.Nombre);

            //ApplyPaging(0, 5);
            ApplyPaging(productoParams.PageSize * (productoParams.PageIndex - 1), productoParams.PageSize);

            if (!string.IsNullOrEmpty(productoParams.Sort))
            {
                switch (productoParams.Sort)
                {
                    case "nombreAsc":
                        AddOrderBy(p => p.Nombre);
                        break;
                    case "nombreDesc":
                        AddOrderByDescending(p => p.Nombre);
                        break;
                    case "precioAsc":
                        AddOrderBy(p => p.Precio);
                        break;
                    case "precioDesc":
                        AddOrderByDescending(p => p.Precio);
                        break;
                    case "descripcionAsc":
                        AddOrderBy(p => p.Descripcion);
                        break;
                    case "descripcionDesc":
                        AddOrderByDescending(p => p.Descripcion);
                        break;
                    default:
                        AddOrderBy(p => p.Nombre);
                        break;
                }
            }
        }

        //Aqui las condiciones con relaciones
        public ProductoWithCategoriaAndMarcaSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(p => p.categoria);
            AddInclude(p => p.marca);
        }


    }
}
