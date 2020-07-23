using Banka.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banka.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            IdentityResult roleResult;
            //Adding Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Administrator");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Administrator"));
            }
            roleCheck = await RoleManager.RoleExistsAsync("Korisnik");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Korisnik"));
            }
            roleCheck = await RoleManager.RoleExistsAsync("Vraboten");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Vraboten"));
            }

            AppUser user1 = await UserManager.FindByEmailAsync("admin@feit.com");
            if (user1 == null)
            {
                var User = new AppUser();
                User.Email = "admin@feit.com";
                User.UserName = "admin@feit.com";
                User.Role = "Administrator";
                string userPWD = "Admin.123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin      
                if (chkUser.Succeeded)
                {
                    var result1 = await UserManager.AddToRoleAsync(User, "Administrator");
                }
            }
        }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BankaContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<BankaContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();
                // Look for any movies.
                if (context.Korisnik.Any() || context.Firma.Any() || context.Employee.Any() || context.KorisnickaSmetka.Any()
                    || context.KompaniskaSmetka.Any() || context.Karticka.Any())
                {
                    return;   // DB has been seeded
                }

                
            }
        }
    }
}
