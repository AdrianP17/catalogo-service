using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace catalogo.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Atributo",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Categoría" },
                    { 2, "Género" },
                    { 3, "Deporte" },
                    { 4, "Tipo" },
                    { 5, "Colección" },
                    { 9, "Talla" },
                    { 10, "Unidad medida" },
                    { 11, "Color" }
                });

            migrationBuilder.InsertData(
                table: "AtributoValor",
                columns: new[] { "Id", "AtributoId", "Valor" },
                values: new object[,]
                {
                    { 1, 1, "Ropa" },
                    { 2, 1, "Calzado" },
                    { 3, 1, "Accesorios" },
                    { 4, 2, "Hombre" },
                    { 5, 2, "Mujer" },
                    { 6, 2, "Niños" },
                    { 7, 3, "Urbano" },
                    { 8, 3, "Futbol" },
                    { 9, 3, "Correr" },
                    { 10, 3, "Entrenar" },
                    { 11, 3, "Tenis" },
                    { 12, 3, "Básquet" },
                    { 13, 3, "Voleibol" },
                    { 14, 3, "Deportes acuáticos" },
                    { 15, 4, "Zapatilla" },
                    { 16, 4, "Chimpunes" },
                    { 17, 4, "Sandalias" },
                    { 18, 4, "Polos" },
                    { 19, 4, "Shorts" },
                    { 20, 4, "Pantalones" },
                    { 21, 4, "Casacas" },
                    { 22, 4, "Ropa de baño" },
                    { 23, 4, "Bolsas y mochilas" },
                    { 24, 4, "Gorras" },
                    { 25, 4, "Medias" },
                    { 26, 4, "Guantes" },
                    { 27, 5, "Colección 2025" },
                    { 28, 11, "Negro" },
                    { 29, 11, "Blanco" },
                    { 30, 11, "Azul" },
                    { 31, 11, "Verde" },
                    { 32, 11, "Plomo" },
                    { 33, 11, "Morado" },
                    { 34, 11, "Rosado" },
                    { 35, 11, "Rojo" },
                    { 36, 11, "Celeste" },
                    { 37, 11, "Naranja" },
                    { 38, 11, "Amarillo" },
                    { 39, 11, "Multicolor" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Atributo",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Atributo",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "AtributoValor",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Atributo",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Atributo",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Atributo",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Atributo",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Atributo",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Atributo",
                keyColumn: "Id",
                keyValue: 11);
        }
    }
}
