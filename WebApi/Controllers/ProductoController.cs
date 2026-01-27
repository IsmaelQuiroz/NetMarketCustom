using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Errors;

namespace WebApi.Controllers
{

    //[Route("api/[controller]")]
    //[ApiController]
    public class ProductoController : BaseApiController
    {

        private readonly IProductoRepository _productoRepository;
        private readonly IMapper _mapper;

        public ProductoController(IProductoRepository productoRepository, IMapper mapper)
        {
            _productoRepository = productoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Producto>>> GetProductos()
        {
            var productos = await _productoRepository.GetProductosAsync();
            //return Ok(productos); // el Ok es un wrapper porque se trata de una lista ReadOnly
            return Ok(_mapper.Map<IReadOnlyList<Producto>, IReadOnlyList<ProductoDto>>(productos)); // el Ok es un wrapper porque se trata de una lista ReadOnly
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetProducto(int id)
        {
            //return  await _productoRepository.GetProductoByIdAsync(id);
            var producto = await _productoRepository.GetProductoByIdAsync(id);

            if(producto ==null)
            {
                //return NotFound(new CodeErrorResponse(404));
                return NotFound(new CodeErrorResponse(404, "El Producto no existe"));
            }

            return  _mapper.Map<Producto, ProductoDto>(producto);
        }

    }
}
