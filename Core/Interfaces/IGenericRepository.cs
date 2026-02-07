using Core.Entities;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    //Interface Generica que procesa las transacciones de nuestra aplicación
    //La T indica que es Genérica la interface y otras clases pueden implementarla
    //el where condiciona que las clases T deben ser hijas de la ClaseBase
    public interface IGenericRepository<T> where T: ClaseBase
    {
        //haremos 2 operaciones genéricas que puedan trabajar para consultar la data de cualquier tipo de entidad tabla a futuro 
        Task<T> GetByIdAsync(int id);
                       
        Task<IReadOnlyList<T>> GetAllAsync();
       
        /*aqui se pasa como parámetro la especificación
         * y dentro de la Specificacion voy incluir las relaciones que va tener T con otras entidades
         * y tambien le incluyo los parametros de busqueda es decir condicion lopgica de la consulta*/
        Task<T> GetByIdWithSpec(ISpecification<T> spec);

        /* cuando se quiera consumir la lista de elementos de la entidad T, 
         * como parametro le estamos pasando las relaciones deben aplicarse a la entidad T y tambien que condiciones
         * debo aplicarle a la consulta */
        Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec);

        //Para la cnatidad de elementos a devolver de la Entidad
        Task<int> CountAsync(ISpecification<T> spec);
    }


}
