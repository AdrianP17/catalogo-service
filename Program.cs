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

builder.Services.AddSingleton<IAlmacenadorArchivos, AzureBlobStorageService>();
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
