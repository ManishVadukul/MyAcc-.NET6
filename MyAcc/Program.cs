using FastReport.Data;
using GridMvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyAcc.Data;
using MyAcc.Initializer;
using MyAcc.Repository;
using MyAcc.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddControllersWithViews();
builder.Services.AddGridMvc();
FastReport.Utils.RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));
//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();




builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<customerRepository>();
builder.Services.AddScoped<supplierRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<PaymentTypeRepository>();
builder.Services.AddScoped<orderRepository>();
builder.Services.AddScoped<reportRepository>();
builder.Services.AddScoped<dashboardRepository>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddSession();
builder.Services.AddRazorPages();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});



var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseFastReport();
app.UseStaticFiles();

app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();
using (var scope = app.Services.CreateScope())
{
    // var services = scope.ServiceProvider;
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

    dbInitializer.Initialize();

}
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
