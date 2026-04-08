using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Data
{
    //Seguridad: 7 Agregar la data inicial, es decir la creación del usuario inicial
    public class SeguridadDbContextData
    {
        public static async Task SeedUserAsync(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager) //Roles 1
        {
            if (!userManager.Users.Any())
            {
                var usuario = new Usuario
                {
                    Nombre = "Maria",
                    Apellido = "Del Carmen",
                    UserName = "mdc",
                    Email = "mdc25@gmail.com",
                    Direccion = new Direccion
                    {
                        Calle = "Los Proceres 321",
                        Ciudad = "México",
                        CodigoPostal = "94350",
                        Departamento = "México"
                    }
                };

                await userManager.CreateAsync(usuario, "Mary2026$");
            }

            //Roles 1.1
            //roleManager administra la tabla de roles de la base de datos
            if (!roleManager.Roles.Any())
            {
                var role = new IdentityRole
                {
                    Name = "ADMIN"
                };

                await roleManager.CreateAsync(role);
            }
        }
    }
}
