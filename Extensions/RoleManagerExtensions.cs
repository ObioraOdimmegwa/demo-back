using System.Threading.Tasks;
using Blazor.Server.Data;
using Microsoft.AspNetCore.Identity;

namespace Blazor.Server.Extensions
{
    public static class RoleManagerExtensions
    {
        public static async Task AddRequiredRoles(this RoleManager<IdentityRole> roleManager)
        {
            // add user role
            if(!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            // add admin role
            if(!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
        }
    }
}