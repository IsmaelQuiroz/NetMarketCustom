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
    public class CategoriaProductoConfiguration : IEntityTypeConfiguration<CategoriaProducto>
    {
        public void Configure(EntityTypeBuilder<CategoriaProducto> builder)
        {
            builder.Property(cp => cp.Nombre).IsRequired().HasMaxLength(200);
            builder.Property(cp => cp.Porcentaje).HasColumnType("decimal(18,4)").HasDefaultValue(0.0);
        }
    }
}
