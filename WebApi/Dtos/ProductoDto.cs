using Core.Entities;

namespace WebApi.Dtos
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Modelo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Costo { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int MinStock { get; set; }
        public int MaxStock { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; }
        public int MarcaId { get; set; }
        public string MarcaNombre { get; set; }
        public string Atributo { get; set; }
        public string OwnerT { get; set; }
        public string Ubicacion { get; set; }
    }
}
