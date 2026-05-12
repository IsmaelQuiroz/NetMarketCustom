using Core.Entities.OrdenCompra;
//using Core.Entities.OrdenCompras;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Data.Configuration
{
    public class OrdenCompraConfiguration : IEntityTypeConfiguration<OrdenCompras>
    {
        public void Configure(EntityTypeBuilder<OrdenCompras> builder)
        {
            //La entidad DireccionEnvio tiene un dueño OrdenCompras
            //No puede existir una direccionEnvio si antes no se crea una Orden de Compra
            //y se indica la relación de 1 a 1 entre la orden de compra y la relación
            builder.OwnsOne(o => o.DireccionEnvio, x =>
            {
                x.WithOwner();
            });

            //para el Status para que se sete como una cadena de texto en la propiedad status que se va generar en mi tabla OrdenCompras de mi BD
            builder.Property(S => S.Status)
                .HasConversion(
                    o => o.ToString(), //origen
                    o => (OrdenStatus)Enum.Parse(typeof(OrdenStatus),o) //Destino
                );

            //si se elimna la ORden de Compra , se deben eliminar en cascada todos los items que la componen
            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);

            builder.Property(o => o.Subtotal)
                .HasColumnType("decimal(18,2)");
               
        }
    }
}
