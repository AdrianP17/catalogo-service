using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace catalogo.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Atributo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atributo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    IdPromocion = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AtributoValor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AtributoId = table.Column<int>(type: "integer", nullable: false),
                    Valor = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtributoValor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtributoValor_Atributo_AtributoId",
                        column: x => x.AtributoId,
                        principalTable: "Atributo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductoId = table.Column<int>(type: "integer", nullable: false),
                    IdCliente = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorito_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductoImagen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductoId = table.Column<int>(type: "integer", nullable: false),
                    Principal = table.Column<bool>(type: "boolean", nullable: false),
                    Imagen = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductoImagen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductoImagen_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Variante",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductoId = table.Column<int>(type: "integer", nullable: false),
                    Precio = table.Column<decimal>(type: "numeric", nullable: false),
                    Sku = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variante", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Variante_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductoAtributo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductoId = table.Column<int>(type: "integer", nullable: false),
                    AtributoValorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductoAtributo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductoAtributo_AtributoValor_AtributoValorId",
                        column: x => x.AtributoValorId,
                        principalTable: "AtributoValor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductoAtributo_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VarianteAtributo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VarianteId = table.Column<int>(type: "integer", nullable: false),
                    AtributoValorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VarianteAtributo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VarianteAtributo_AtributoValor_AtributoValorId",
                        column: x => x.AtributoValorId,
                        principalTable: "AtributoValor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VarianteAtributo_Variante_VarianteId",
                        column: x => x.VarianteId,
                        principalTable: "Variante",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VarianteImagen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VarianteId = table.Column<int>(type: "integer", nullable: false),
                    VarianteAtributoId = table.Column<int>(type: "integer", nullable: false),
                    Imagen = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VarianteImagen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VarianteImagen_VarianteAtributo_VarianteAtributoId",
                        column: x => x.VarianteAtributoId,
                        principalTable: "VarianteAtributo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VarianteImagen_Variante_VarianteId",
                        column: x => x.VarianteId,
                        principalTable: "Variante",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AtributoValor_AtributoId",
                table: "AtributoValor",
                column: "AtributoId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorito_ProductoId",
                table: "Favorito",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductoAtributo_AtributoValorId",
                table: "ProductoAtributo",
                column: "AtributoValorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductoAtributo_ProductoId",
                table: "ProductoAtributo",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductoImagen_ProductoId",
                table: "ProductoImagen",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Variante_ProductoId",
                table: "Variante",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_VarianteAtributo_AtributoValorId",
                table: "VarianteAtributo",
                column: "AtributoValorId");

            migrationBuilder.CreateIndex(
                name: "IX_VarianteAtributo_VarianteId",
                table: "VarianteAtributo",
                column: "VarianteId");

            migrationBuilder.CreateIndex(
                name: "IX_VarianteImagen_VarianteAtributoId",
                table: "VarianteImagen",
                column: "VarianteAtributoId");

            migrationBuilder.CreateIndex(
                name: "IX_VarianteImagen_VarianteId",
                table: "VarianteImagen",
                column: "VarianteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorito");

            migrationBuilder.DropTable(
                name: "ProductoAtributo");

            migrationBuilder.DropTable(
                name: "ProductoImagen");

            migrationBuilder.DropTable(
                name: "VarianteImagen");

            migrationBuilder.DropTable(
                name: "VarianteAtributo");

            migrationBuilder.DropTable(
                name: "AtributoValor");

            migrationBuilder.DropTable(
                name: "Variante");

            migrationBuilder.DropTable(
                name: "Atributo");

            migrationBuilder.DropTable(
                name: "Producto");
        }
    }
}
