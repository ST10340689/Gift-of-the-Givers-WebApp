using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using System.Text.Json;
using System.IO;

var contentRoot = Directory.GetCurrentDirectory();
var configFile = Path.Combine(contentRoot, "appsettings.json");

if (File.Exists(configFile))
{
    try
    {
        using var fs = File.OpenRead(configFile);
        JsonDocument.Parse(fs); // will throw JsonException on invalid JSON
    }
    catch (JsonException ex)
    {
        var backup = configFile + $".invalid-{DateTime.Now:yyyyMMddHHmmss}.bak";
        try
        {
            File.Move(configFile, backup);
        }
        catch
        {
            // If move fails for any reason, attempt a safe copy/replace
            File.Copy(configFile, backup, overwrite: true);
            File.Delete(configFile);
        }

        File.WriteAllText(configFile, "{}"); // minimal valid JSON to allow startup
        Console.Error.WriteLine($"Invalid JSON in '{configFile}': {ex.Message}. Backed up to '{backup}' and replaced with '{{}}'. Fix the original file to restore settings.");
    }
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (dbContext.Database.IsRelational())
    {
        // DB initialization / migrations can go here

    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();

app.UseAuthentication(); // Authn/Authz middleware requires corresponding service registrations
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Register}/{id?}");


app.Run();
