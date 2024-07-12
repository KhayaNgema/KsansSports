using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.AspNetCore;
using MyField.Data;
using MyField.Models;
using MyField.Services;
using MyField.Interfaces;
using System.Globalization;
using System.Reflection;




public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
       
        var connectionString = Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<Ksans_SportsDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddDefaultIdentity<UserBaseModel>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<Ksans_SportsDbContext>();

        services.AddAutoMapper(typeof(Startup));
        services.AddControllersWithViews();
        services.AddRazorPages();

        services.AddHttpContextAccessor();



        SetCulture("en-US");

        services.AddScoped<IViewRenderService, ViewRenderService>();
        services.AddScoped<IActivityLogger, ActivityLogger>();
        services.AddScoped<FileUploadService>();
        services.AddScoped<PdfService>();
        services.AddScoped<RandomPasswordGeneratorService>();
        services.AddScoped<EmailService>();
        services.AddScoped<IPaymentService, PayFastPaymentService>();
        services.AddHttpClient<DeviceInfoService>();
        services.AddHttpClient();
/*        services.AddHostedService<FixtureSchedulerHostedService>();
        services.AddScoped<FixtureService>();*/



        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders(); // Clear the default providers
            loggingBuilder.AddSerilog(dispose: true); // Add Serilog
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Serilog configuration
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("Logs/myapp-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSerilogRequestLogging(); 

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });

        app.ApplicationServices.CreateRolesAndDefaultUser().Wait();


    }

    private void SetCulture(string cultureCode)
    {
        var culture = CultureInfo.CreateSpecificCulture(cultureCode);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }


}
