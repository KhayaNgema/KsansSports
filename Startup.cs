﻿using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyField.Data;
using MyField.Helpers;
using MyField.Interfaces;
using MyField.Models;
using MyField.Services;
using Serilog;
using System;
using System.Globalization;
using System.Security.Claims;
using System.Threading;
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


        var key = Configuration["AES_KEY"];
        var iv = Configuration["AES_IV"];

        var keyBytes = Convert.FromBase64String(key);
        var ivBytes = Convert.FromBase64String(iv);

        services.AddSingleton(new EncryptionConfiguration { Key = keyBytes, Iv = ivBytes });

        services.AddScoped<IEncryptionService, EncryptionService>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AnyRole", policy =>
                policy.RequireAssertion(context => context.User.Identity.IsAuthenticated &&
                                                   context.User.Claims.Any(c => c.Type == ClaimTypes.Role)));
        });

        services.AddDistributedMemoryCache();
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        services.AddDbContext<Ksans_SportsDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddDefaultIdentity<UserBaseModel>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<Ksans_SportsDbContext>();

        services.AddScoped<SignInManager<UserBaseModel>>();

        services.AddScoped<UserManager<UserBaseModel>>();

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
        services.AddScoped<LiveTimeService>();

        services.AddScoped<FixtureService>();
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true,
                UsePageLocksOnDequeue = true,
                SchemaName = "hangfire"
            }));

        services.AddHangfireServer();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(dispose: true);
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }


        app.UseSession();

        app.Use(async (context, next) =>
        {
            context.Response.Cookies.Append("StrictlyNecessaryCookie", "Value", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
            await next.Invoke();
        });

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

        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[] { new HangfireAuthorizationFilter() }
        });

        app.UseSerilogRequestLogging();

        RecurringJob.AddOrUpdate<FixtureService>(
            "schedule-fixtures",
            service => service.ScheduleFixturesAsync(),
            Cron.Weekly(DayOfWeek.Monday, 0, 0));

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