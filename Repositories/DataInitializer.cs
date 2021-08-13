using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public class DataInitializer
    {
        public static void SeedData(UserManager<ToDoUser> userManager, RoleManager<Role> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<ToDoUser> userManager)
        {
            if (userManager.FindByNameAsync("SuperAdmin").Result == null)
            {
                ToDoUser user = new ToDoUser();
                user.UserName = "SuperAdmin";
                user.Email = "salocalhost@gmail.com";
                user.FirstName = "Demo Super";
                user.LastName = "Admin";

                IdentityResult result = userManager.CreateAsync
                (user, "Qa@123454").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "SuperAdmin").Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<Role> roleManager)
        {
            if (!roleManager.RoleExistsAsync("SuperAdmin").Result)
            {
                Role role = new Role();
                role.Name = "SuperAdmin";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                Role role = new Role();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                Role role = new Role();
                role.Name = "User";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
    }
}
