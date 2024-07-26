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

            string[] roleNames = { "Club Administrator", "Club Manager", "Player", "Sport Administrator", "News Updator", "Sport Coordinator", "System Administrator", "Official", "News Administrator", "Fans Administrator", "Personnel Administrator", "Sport Manager" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
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
                    await userManager.AddToRoleAsync(defaultUser, "Personnel Administrator");
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
