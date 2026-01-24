using A_Mover_Desktop_Final.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>() // Add roles to the identity system
.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = true;      // tens a inicial maiúscula
    options.Password.RequireLowercase = true;      // "mecanico" já cumpre
    options.Password.RequireNonAlphanumeric = true; // o "." cumpre
    options.Password.RequiredLength = 8;           // ajusta se precisares
});

// Adicione esta linha ao seu Program.cs antes de "var app = builder.Build();"
builder.Services.AddHttpClient();

var app = builder.Build();

// Seed roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        await SeedRolesAsync(services, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

// Usar CORS
app.UseCors("AllowAll");

app.UseAuthentication(); // Ensure authentication is used
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");
app.MapRazorPages();

app.Run();

async Task SeedRolesAsync(IServiceProvider serviceProvider, ILogger logger)
{
    // Get the role manager service
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Define the roles you want to create
    var roles = new[] { "Oficina", "Fabricante", "Concessionaria" };

    foreach (var role in roles)
    {
        // Check if the role already exists; if not, create it
        if (!await roleManager.RoleExistsAsync(role))
        {
            var result = await roleManager.CreateAsync(new IdentityRole(role));
            if (result.Succeeded)
            {
                logger.LogInformation("Role {Role} created successfully.", role);
            }
            else
            {
                logger.LogError("Error creating role {Role}: {Errors}", role, string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
