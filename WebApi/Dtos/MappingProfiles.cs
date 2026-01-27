using AutoMapper;
using Core.Entities;

namespace WebApi.Dtos
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() {
            //Mapeo Automatico
            //CreateMap<Producto, ProductoDto>();

            //Mapeo especificado
            CreateMap<Producto, ProductoDto>()
                .ForMember(p => p.CategoriaNombre, x => x.MapFrom(a => a.categoria.Nombre))
                .ForMember(p => p.MarcaNombre, x => x.MapFrom(a => a.marca.Nombre));
        }
    }
}
