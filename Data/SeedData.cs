using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using MyField.Models;
using MyField.Data;
using Microsoft.EntityFrameworkCore;

public static class SeedData
{
    public static async Task CreateRolesAndDefaultUser(this IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserBaseModel>>();
            var context = scope.ServiceProvider.GetRequiredService<Ksans_SportsDbContext>();

            string[] roleNames = { "Club Administrator", "Club Manager", "Player", "Sport Administrator", "News Updator", "Sport Coordinator", "System Administrator", "Official", "News administrator", "Fans administrator", "Personnel administrator", "Sport manager" };

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
                    CreatedBy = "default-user-id",
                    ModifiedBy = "default-user-id"
                };

                var result = await userManager.CreateAsync(defaultUser, "Admin@123");

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

            var overallNewsReport = new OverallNewsReport
            {
                AuthoredNewsCount = 0,
                ApprovedNewsCount = 0,
                PublishedNewsCount = 0,
                RejectedNewsCount = 0,
                NewsReadersCount = 0,
                ApprovedNewsRate = 0,
                PublishedNewsRate = 0,
                RejectedNewsRate = 0,
            };

            if (!await context.OverallNewsReports.AnyAsync())
            {
                context.OverallNewsReports.Add(overallNewsReport);
                await context.SaveChangesAsync();
            }

            var personnelAccountsReport = new PersonnelAccountsReport
            {
                OverallAccountsCount = 0,
                ActiveAccountsCount = 0,
                InactiveAccountsCount = 0,
                SuspendedAccountsCount = 0,
                ActiveAccountsRate = 0,
                InactiveAccountsRate = 0,
                SuspendedAccountsRate = 0,
            };

            if (!await context.PersonnelAccountsReports.AnyAsync())
            {
                context.PersonnelAccountsReports.Add(personnelAccountsReport);
                await context.SaveChangesAsync();
            }

            var personnelFinancialReport = new PersonnelFinancialReport
            {
                ExpectedRepayableAmount = 0,
                PaidPaymentFinesCount = 0,
                PendingPaymentFinesCount = 0,
                OverduePaymentFineCount = 0,
                TotalPaidAmount = 0,
                TotalUnpaidAmount = 0,
                RepayableFinesCount = 0,
                OverdueFinesRate = 0,
                PaidFinesRate = 0,  
                PendingFinesRate = 0
            };

            if (!await context.PersonnelFinancialReports.AnyAsync())
            {
                context.PersonnelFinancialReports.Add(personnelFinancialReport);
                await context.SaveChangesAsync();
            }


            var fansAccountsReport = new FansAccountsReport
            {
               OverallFansAccountsCount = 0,
               ActiveFansAccountsCount = 0,
               InactiveFansAccountsCount = 0,
               SuspendedFansAccountsCount = 0,
               ActiveFansAccountsRate = 0,
               InactiveFansAccountsRate = 0,
               SuspendedFansAccountsRate = 0
            };

            if (!await context.FansAccountsReports.AnyAsync())
            {
                context.FansAccountsReports.Add(fansAccountsReport);
                await context.SaveChangesAsync();
            }
        }
    }
}
