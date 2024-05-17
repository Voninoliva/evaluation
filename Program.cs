using btp.Models.Data;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Session
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; 
    
});



// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BtpContext>(options =>
{
    // Configurez la connexion à votre base de données
    options.UseNpgsql(builder.Configuration.GetConnectionString("mot"));
    
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
     app.UseHsts();
     RotativaConfiguration.Setup(@"D:\itu\s6\eval\btp\wwwroot\Rotativa");

}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRotativa();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
