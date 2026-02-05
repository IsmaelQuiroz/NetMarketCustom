using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    //esta interface representará las operaciones que se podrán realizar 
    public interface ISpecification<T>
    {

        /* Criteria Es la condición lógica, Filtros que se quiere aplicar a una Entidad-Consulta
           T:       Entidad generica sobre la cual va trabajar el Func
           bool:    Se va devolver un valor booleano  
        */
        Expression<Func<T, bool>> Criteria { get;}


        /* Includes: es una lista que representa las relaciones a implementar en las tablas de la consulta,
         * relacionadas a la Entidad
            T:      Entindad con la que quiero trabajar
            object: valor devuelto es un objet
         */
         List<Expression<Func<T, object>>> Includes { get;}

        //Para el ordenamiento, T, object ( objeto que va devolver) 
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }

    }
}
