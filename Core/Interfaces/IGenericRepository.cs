using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    //La T indica que es Genérica la interface y otras clases pueden implementarla
    //el where condiciona que las clases T deben ser hijas de la ClaseBase
    public interface IGenericRepository<T> where T: ClaseBase
    {
        //haremos 2 operaciones genéricas que puedan trabajar para consultar la data de cualquier tipo de entidad tabla a futuro 
        Task<T> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> GetAllAsync();
    }
}
