using catalogo.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using DotNetEnv;
using catalogo.Repository;
using catalogo.Services;
using catalogo.Interfaces.IServices;
using catalogo.Interfaces.IRepositories;
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    // Si la cadena de conexión no se encuentra en 'DefaultConnection' (lo que Azure inyecta), 
    // entonces intenta obtenerla directamente como variable de entorno (para el caso de dotnet env o Docker)
    connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
}

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string is missing. Set 'DefaultConnection' or 'DATABASE_URL'.");
}

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Repositories
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IAtributoRepository, AtributoRepository>();
builder.Services.AddScoped<IAtributoValorRepository, AtributoValorRepository>();
builder.Services.AddScoped<IVarianteRepository, VarianteRepository>();

// Services
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IAtributoService, AtributoService>();
builder.Services.AddScoped<IAtributoValorService, AtributoValorService>();
builder.Services.AddScoped<IVarianteService, VarianteService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDBContext>();

        context.Database.Migrate(); 

        Console.WriteLine("Database migration and seeding successful.");
    }
    catch (Exception ex)
    {
        // Logging de errores es crucial si falla la migración
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        // Si el curso lo permite, podrías detener la aplicación aquí
        // throw; 
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
