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
    public class VentaConfiguration : IEntityTypeConfiguration<Venta>
    {
        public void Configure(EntityTypeBuilder<Venta> builder)
        {
            builder.Property(d => d.Fecha).HasColumnType("datetime");
            builder.Property(v => v.Monto).HasColumnType("decimal(18,4)");
            builder.HasOne(cl => cl.Cliente).WithMany().HasForeignKey(v => v.ClienteId);
            //builder.HasOne(cp => cp.CategoriaProducto).WithMany().HasForeignKey(c => c.CategoriaProductoId);
        }
    }
}
