using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using MyField.Models;

public static class SeedData
{
    public static async Task CreateRolesAndDefaultUser(this IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserBaseModel>>();

            string[] roleNames = { "Club Administrator", 
                "Club Manager", 
                "Player", 
                "Sport Administrator", 
                "News Updator", 
                "Sport Coordinator", 
                "System Administrator", 
                "Official", 
                "News administrator", 
                "Fans administrator",
                "Personnel administrator", 
                "Sport manager" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }


            var systemAdmin = await userManager.FindByEmailAsync("admin@gmail.com");
            if (systemAdmin == null)
            {
                // Create a new UserBaseModel instance (not SystemAdministrator)
                var defaultUser = new SportsMember
                {
                    FirstName = "Main",
                    LastName = "Admin",
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    PhoneNumber = "0660278127",
                    DateOfBirth = DateTime.Now,
                    ProfilePicture = "khaya.jpg",
                    EmailConfirmed = true,
                    CreatedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                    CreatedBy = "default-user-id", // set the default user ID
                    ModifiedBy = "default-user-id" // set the default user ID
                };

                // Create the user with the specified password
                var result = await userManager.CreateAsync(defaultUser, "Admin@123");

                // If user creation is successful, assign the System Administrator role
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, "System Administrator");
                }
            }


            var sportsAdmin = await userManager.FindByEmailAsync("Sportadmin@gmail.com");
            if (sportsAdmin == null)
            {
                var defaultUser = new SportsMember
                {
                    FirstName = "Sport",
                    LastName = "Admin",
                    UserName = "Sportadmin@gmail.com",
                    Email = "Sportadmin@gmail.com",
                    PhoneNumber = "0660278127",
                    DateOfBirth = DateTime.Now,
                    ProfilePicture = "khaya.jpg",
                    EmailConfirmed = true,
                    CreatedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                    CreatedBy = "default-user-id",
                    ModifiedBy = "default-user-id" 
                };

                var result = await userManager.CreateAsync(defaultUser, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, "Sport Administrator");
                }
            }

            var sportCoordinator = await userManager.FindByEmailAsync("codi@gmail.com");
            if (sportCoordinator == null)
            {
                var defaultUser = new SportsMember
                {
                    FirstName = "Sport",
                    LastName = "Coordinator",
                    UserName = "codi@gmail.com",
                    Email = "codi@gmail.com",
                    PhoneNumber = "0660278127",
                    DateOfBirth = DateTime.Now,
                    ProfilePicture = "khaya.jpg",
                    EmailConfirmed = true,
                    CreatedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                    CreatedBy = "default-user-id",
                    ModifiedBy = "default-user-id"
                };

                var result = await userManager.CreateAsync(defaultUser, "Codi@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, "Sport Coordinator");
                }
            }

            var official = await userManager.FindByEmailAsync("official@gmail.com");
            if (official == null)
            {
                var defaultUser = new Officials
                {
                    FirstName = "Official",
                    UserName = "official@gmail.com",
                    Email = "official@gmail.com",
                    PhoneNumber = "0660278127",
                    DateOfBirth = DateTime.Now,
                    ProfilePicture = "khaya.jpg",
                    EmailConfirmed = true,
                    CreatedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                    CreatedBy = "default-user-id",
                    ModifiedBy = "default-user-id"
                };

                var result = await userManager.CreateAsync(defaultUser, "Official@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, "Official");
                }
            }

            var newsUpdator = await userManager.FindByEmailAsync("sportnews@gmail.com");
            if (newsUpdator == null)
            {
                var defaultUser = new SportsMember
                {
                    FirstName = "Khayalethu",
                    LastName = "Msweli",
                    UserName = "sportnews@gmail.com",
                    Email = "sportnews@gmail.com",
                    PhoneNumber = "0660278127",
                    DateOfBirth = DateTime.Now,
                    ProfilePicture = "khaya.jpg",
                    EmailConfirmed = true,
                    CreatedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                    CreatedBy = "default-user-id",
                    ModifiedBy = "default-user-id"
                };

                var result = await userManager.CreateAsync(defaultUser, "News@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, "News Updator");
                }
            }

            var newsAdmin = await userManager.FindByEmailAsync("sportnewsadmin@gmail.com");
            if (newsAdmin == null)
            {
                var defaultUser = new SportsMember
                {
                    FirstName = "Khayalethu",
                    LastName = "Msweli",
                    UserName = "sportnewsadmin@gmail.com",
                    Email = "sportnewsadmin@gmail.com",
                    PhoneNumber = "0660278127",
                    DateOfBirth = DateTime.Now,
                    ProfilePicture = "khaya.jpg",
                    EmailConfirmed = true,
                    CreatedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                    CreatedBy = "default-user-id",
                    ModifiedBy = "default-user-id"
                };

                var result = await userManager.CreateAsync(defaultUser, "News@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, "News administrator");
                }
            }

            var personnelAdmin = await userManager.FindByEmailAsync("personnel@gmail.com");
            if (personnelAdmin == null)
            {
                var defaultUser = new SportsMember
                {
                    FirstName = "Khayalethu",
                    LastName = "Msweli",
                    UserName = "personnel@gmail.com",
                    Email = "personnel@gmail.com",
                    PhoneNumber = "0660278127",
                    DateOfBirth = DateTime.Now,
                    ProfilePicture = "khaya.jpg",
                    EmailConfirmed = true,
                    CreatedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                    CreatedBy = "default-user-id",
                    ModifiedBy = "default-user-id"
                };

                var result = await userManager.CreateAsync(defaultUser, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, "Personnel administrator");
                }
            }

            var fansAdmin = await userManager.FindByEmailAsync("fansadmin@gmail.com");
            if (fansAdmin == null)
            {
                var defaultUser = new SportsMember
                {
                    FirstName = "Khayalethu",
                    LastName = "Msweli",
                    UserName = "fansadmin@gmail.com",
                    Email = "fansadmin@gmail.com",
                    PhoneNumber = "0660278127",
                    DateOfBirth = DateTime.Now,
                    ProfilePicture = "khaya.jpg",
                    EmailConfirmed = true,
                    CreatedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                    CreatedBy = "default-user-id",
                    ModifiedBy = "default-user-id"
                };

                var result = await userManager.CreateAsync(defaultUser, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, "Fans administrator");
                }
            }
        }


    }
}