using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.EntityFrameworkCore;
using TimeReporter.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews()
.AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddDistributedMemoryCache();

builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate();


var connectionString = "server=localhost;port=3306;user=root;password=root;database=TimeReporterReact";

var serverVersion = new MySqlServerVersion(ServerVersion.AutoDetect(connectionString));

builder.Services.AddDbContextPool<TimeReporterContext>(dbContextOptions =>
                dbContextOptions.UseMySql(connectionString, serverVersion));

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".TimeReporter.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();

app.UseCookiePolicy();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");;

app.Run();
