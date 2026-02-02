using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly IGenericRepository<CategoriaProducto> _categoriaRepository;

        public CategoriaController(IGenericRepository<CategoriaProducto> categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CategoriaProducto>>> GetCategoriaAll()
        {
            return Ok(await _categoriaRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaProducto>> GetCategoriaById(int id)
        {
            return await _categoriaRepository.GetByIdAsync(id);
        }

    }
}
