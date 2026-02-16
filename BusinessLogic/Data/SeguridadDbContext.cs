using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Data
{
    //Seguridad:2
    public class SeguridadDbContext : IdentityDbContext<Usuario> //En este context se encuentra todo el esquema de seguridad como tablas de Roles,  roles-usuarios
    {
        public SeguridadDbContext(DbContextOptions<SeguridadDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        //En este caso no se requieren agregar nuevas entidades  para la seguirdad
        //Se va trabajar con las que ya se tienen en el modelo IdentityCore
    }
}
