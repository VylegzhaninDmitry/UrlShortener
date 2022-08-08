using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using UrlShortener.Models;
using UrlShortener.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<UrlContext>(opt => opt.UseMySql(builder.Configuration.GetConnectionString("MariaDbConnectionString"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MariaDbConnectionString"))));
builder.Services.AddScoped<IUrlRepository, UrlRepository>();
var app = builder.Build();

//AutoMigration
using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>()
.CreateScope())
{
    using var context = serviceScope.ServiceProvider.GetService<UrlContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors(builder => builder.AllowAnyOrigin());
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
