using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Data.Configuration
{
    public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.Property(p => p.Nombre).IsRequired().HasMaxLength(250);
            builder.Property(p => p.Descripcion).IsRequired().HasMaxLength(500);
            //builder.Property(p => p.Imagen).HasMaxLength(1000);
            builder.Property(p => p.Precio).HasColumnType("decimal(18,2)");
            builder.Property(p => p.Costo).HasColumnType("decimal(18,2)");

            //Llave foranea del producto con la marca
            builder.HasOne(m => m.categoria).WithMany().HasForeignKey(p => p.CategoriaId);
            builder.HasOne(m => m.marca).WithMany().HasForeignKey(p => p.MarcaId);
        }
    }
}
