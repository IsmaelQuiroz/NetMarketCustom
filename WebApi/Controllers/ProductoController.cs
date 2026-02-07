using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Errors;

namespace WebApi.Controllers
{

    //[Route("api/[controller]")]
    //[ApiController]
    public class ProductoController : BaseApiController
    {

        //private readonly IProductoRepository _productoRepository;

        // la <T> se transforma en Producto en este contexto
        private readonly IGenericRepository<Producto> _productoRepository;
        private readonly IMapper _mapper;

        //public ProductoController(IProductoRepository productoRepository, IMapper mapper)
        public ProductoController(IGenericRepository<Producto> productoRepository, IMapper mapper)
        {
            _productoRepository = productoRepository;
            _mapper = mapper;
        }

        //http://localhost:5000/api/Producto
        //http://localhost:5000/api/Producto?sort=descripcionAsc&marca=10
        //http://localhost:5000/api/Producto?pageIndex=1&pageSize=2

        [HttpGet] 
        //public async Task<ActionResult<List<Producto>>> GetProductos(string sort, int? marca, int? categoria, int pageIndex)
        //public async Task<ActionResult<List<Producto>>> GetProductos([FromQuery]ProductoSpecificationParams productoParams)
        public async Task<ActionResult<Pagination<ProductoDto>>> GetProductos([FromQuery] ProductoSpecificationParams productoParams)
        {
            //var spec =  new ProductoWithCategoriaAndMarcaSpecification(sort, marca, categoria);
            var spec = new ProductoWithCategoriaAndMarcaSpecification(productoParams);
            //var productos = await _productoRepository.GetProductosAsync();
            //var productos = await _productoRepository.GetAllAsync();
            var productos = await _productoRepository.GetAllWithSpec(spec);

            var specCount = new ProductoForCountingSpecification(productoParams);
            var totalProductos = await _productoRepository.CountAsync(specCount);

            var rounded = Math.Ceiling(Convert.ToDecimal(totalProductos / productoParams.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<Producto>, IReadOnlyList<ProductoDto>>(productos);

            return Ok(
                new Pagination<ProductoDto>
                {
                    Count = totalProductos,
                    Data = data,
                    PageCount = totalPages,
                    PageIndex = productoParams.PageIndex,
                    PageSize = productoParams.PageSize
                }
            );

            //return Ok(productos); // el Ok es un wrapper porque se trata de una lista ReadOnly
            //return Ok(_mapper.Map<IReadOnlyList<Producto>, IReadOnlyList<ProductoDto>>(productos)); // el Ok es un wrapper porque se trata de una lista ReadOnly
        }

        //http://localhost:5000/api/Producto/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetProducto(int id)
        {
            //return  await _productoRepository.GetProductoByIdAsync(id);
            //var producto = await _productoRepository.GetProductoByIdAsync(id);           
            //var producto = await _productoRepository.GetByIdAsync(id);

            //spec = debe incluir la logica de la condicion de la consulta y las relaciones entre
            //las entidades, la relación entre Producto, Marca y Categoría.

            var spec = new ProductoWithCategoriaAndMarcaSpecification(id);
            var producto = await _productoRepository.GetByIdWithSpec(spec);

            if (producto ==null)
            {
                //return NotFound(new CodeErrorResponse(404));
                return NotFound(new CodeErrorResponse(404, "El Producto no existe"));
            }

            return  _mapper.Map<Producto, ProductoDto>(producto);
        }

    }
}
