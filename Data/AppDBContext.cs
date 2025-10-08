using catalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace catalogo.Data;

public class AppDBContext : DbContext
{

    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }


    //Create DbSet for each model
    //Ex: public DbSet<Product> Products { get; set; }
    public DbSet<Producto> Producto { get; set; } = null!;
    public DbSet<ProductoImagen> ProductoImagen { get; set; } = null!;
    public DbSet<Variante> Variante { get; set; } = null!;
    public DbSet<VarianteImagen> VarianteImagen { get; set; } = null!;
    public DbSet<VarianteAtributo> VarianteAtributo { get; set; } = null!;
    public DbSet<Atributo> Atributo { get; set; } = null!;
    public DbSet<AtributoValor> AtributoValor { get; set; } = null!;
    public DbSet<ProductoAtributo> ProductoAtributo { get; set; } = null!;
    public DbSet<Favorito> Favorito { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Atributo>().HasData(
         new Atributo { Id = 1, Nombre = "Categoría" },
         new Atributo { Id = 2, Nombre = "Género" },
         new Atributo { Id = 3, Nombre = "Deporte" },
         new Atributo { Id = 4, Nombre = "Tipo" },
         new Atributo { Id = 5, Nombre = "Colección" },
         new Atributo { Id = 9, Nombre = "Talla" },
         new Atributo { Id = 10, Nombre = "Unidad medida" },
         new Atributo { Id = 11, Nombre = "Color" }
     );

        modelBuilder.Entity<AtributoValor>().HasData(
                new AtributoValor { Id = 1, Valor = "Ropa", AtributoId = 1 },
                new AtributoValor { Id = 2, Valor = "Calzado", AtributoId = 1 },
                new AtributoValor { Id = 3, Valor = "Accesorios", AtributoId = 1 },
                new AtributoValor { Id = 4, Valor = "Hombre", AtributoId = 2 },
                new AtributoValor { Id = 5, Valor = "Mujer", AtributoId = 2 },
                new AtributoValor { Id = 6, Valor = "Niños", AtributoId = 2 },
                new AtributoValor { Id = 7, Valor = "Urbano", AtributoId = 3 },
                new AtributoValor { Id = 8, Valor = "Futbol", AtributoId = 3 },
                new AtributoValor { Id = 9, Valor = "Correr", AtributoId = 3 },
                new AtributoValor { Id = 10, Valor = "Entrenar", AtributoId = 3 },
                new AtributoValor { Id = 11, Valor = "Tenis", AtributoId = 3 },
                new AtributoValor { Id = 12, Valor = "Básquet", AtributoId = 3 },
                new AtributoValor { Id = 13, Valor = "Voleibol", AtributoId = 3 },
                new AtributoValor { Id = 14, Valor = "Deportes acuáticos", AtributoId = 3 },
                new AtributoValor { Id = 15, Valor = "Zapatilla", AtributoId = 4 },
                new AtributoValor { Id = 16, Valor = "Chimpunes", AtributoId = 4 },
                new AtributoValor { Id = 17, Valor = "Sandalias", AtributoId = 4 },
                new AtributoValor { Id = 18, Valor = "Polos", AtributoId = 4 },
                new AtributoValor { Id = 19, Valor = "Shorts", AtributoId = 4 },
                new AtributoValor { Id = 20, Valor = "Pantalones", AtributoId = 4 },
                new AtributoValor { Id = 21, Valor = "Casacas", AtributoId = 4 },
                new AtributoValor { Id = 22, Valor = "Ropa de baño", AtributoId = 4 },
                new AtributoValor { Id = 23, Valor = "Bolsas y mochilas", AtributoId = 4 },
                new AtributoValor { Id = 24, Valor = "Gorras", AtributoId = 4 },
                new AtributoValor { Id = 25, Valor = "Medias", AtributoId = 4 },
                new AtributoValor { Id = 26, Valor = "Guantes", AtributoId = 4 },
                new AtributoValor { Id = 27, Valor = "Colección 2025", AtributoId = 5 },
                new AtributoValor { Id = 28, Valor = "Negro", AtributoId = 11 },
                new AtributoValor { Id = 29, Valor = "Blanco", AtributoId = 11 },
                new AtributoValor { Id = 30, Valor = "Azul", AtributoId = 11 },
                new AtributoValor { Id = 31, Valor = "Verde", AtributoId = 11 },
                new AtributoValor { Id = 32, Valor = "Plomo", AtributoId = 11 },
                new AtributoValor { Id = 33, Valor = "Morado", AtributoId = 11 },
                new AtributoValor { Id = 34, Valor = "Rosado", AtributoId = 11 },
                new AtributoValor { Id = 35, Valor = "Rojo", AtributoId = 11 },
                new AtributoValor { Id = 36, Valor = "Celeste", AtributoId = 11 },
                new AtributoValor { Id = 37, Valor = "Naranja", AtributoId = 11 },
                new AtributoValor { Id = 38, Valor = "Amarillo", AtributoId = 11 },
                new AtributoValor { Id = 39, Valor = "Multicolor", AtributoId = 11 }
            );
    }
}
