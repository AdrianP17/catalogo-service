using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace catalogo.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVarianteImagen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VarianteImagen_VarianteAtributo_VarianteAtributoId",
                table: "VarianteImagen");

            migrationBuilder.DropIndex(
                name: "IX_VarianteImagen_VarianteAtributoId",
                table: "VarianteImagen");

            migrationBuilder.DropColumn(
                name: "VarianteAtributoId",
                table: "VarianteImagen");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VarianteAtributoId",
                table: "VarianteImagen",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VarianteImagen_VarianteAtributoId",
                table: "VarianteImagen",
                column: "VarianteAtributoId");

            migrationBuilder.AddForeignKey(
                name: "FK_VarianteImagen_VarianteAtributo_VarianteAtributoId",
                table: "VarianteImagen",
                column: "VarianteAtributoId",
                principalTable: "VarianteAtributo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
