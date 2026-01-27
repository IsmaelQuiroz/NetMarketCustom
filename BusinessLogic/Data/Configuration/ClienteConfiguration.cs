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
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.Property(cl => cl.Nombre).IsRequired().HasMaxLength(450);
            builder.Property(cl => cl.email).IsRequired().HasMaxLength(250);
            builder.Property(cl => cl.Telefono).HasMaxLength(50);
            builder.Property(cl => cl.Direccion).HasMaxLength(600);
            builder.Property(cl => cl.Imagen).HasMaxLength(1000);
        }
    }
}
