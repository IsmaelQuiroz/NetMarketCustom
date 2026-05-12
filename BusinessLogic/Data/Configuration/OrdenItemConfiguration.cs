using Core.Entities.OrdenCompra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Data.Configuration
{
    public class OrdenItemConfiguration : IEntityTypeConfiguration<OrdenItem>
    {
        public void Configure(EntityTypeBuilder<OrdenItem> builder)
        {
            //relacion de uno a uno, la i es la repsentación de la entidad OrdenItem
            builder.OwnsOne(i => i.ItemOrdenado,
                                                    x => {x.WithOwner(); } ); //el ancla de la relación entre el padre y el hijo

            builder.Property(i => i.Precio)
                .HasColumnType("decimal(18,2)");
        }
    }
}
