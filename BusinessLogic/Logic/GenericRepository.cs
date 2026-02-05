using BusinessLogic.Data;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Logic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ClaseBase
    {
        private readonly MarketDbContext _context;

        public GenericRepository(MarketDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
           return await _context.Set<T>().FindAsync(id);
        }


        //Estos metodos serán consumidos por el controller, dentro del controler se va crear el objeto spec 

        //los parametros condiciones y relaciones entre entidades vienen desde el objeto spec
        public async Task<T> GetByIdWithSpec(ISpecification<T> spec)
        {
            //en la implementacion del spec, va pasar hacia el AppluSpecification 
            //e implementará toda la lógica para retornar un solo valor
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

       
        public async Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        //Metodo a utilizar para poder devolver la data de la entidad Genérica
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            //Las condiciones e incldes dinamicamente se van a agregar dentro del:
            //SpecificationEvaluator
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }
    }
}
