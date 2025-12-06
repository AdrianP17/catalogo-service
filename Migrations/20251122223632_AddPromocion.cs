using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace catalogo.Migrations
{
    /// <inheritdoc />
    public partial class AddPromocion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdPromocion",
                table: "Producto",
                newName: "PromocionId");

            migrationBuilder.CreateTable(
                name: "Promociones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Monto = table.Column<decimal>(type: "numeric", nullable: false),
                    Porcentaje = table.Column<decimal>(type: "numeric", nullable: false),
                    Fecha_inicio = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Fecha_limite = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promociones", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Producto_PromocionId",
                table: "Producto",
                column: "PromocionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Producto_Promociones_PromocionId",
                table: "Producto",
                column: "PromocionId",
                principalTable: "Promociones",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producto_Promociones_PromocionId",
                table: "Producto");

            migrationBuilder.DropTable(
                name: "Promociones");

            migrationBuilder.DropIndex(
                name: "IX_Producto_PromocionId",
                table: "Producto");

            migrationBuilder.RenameColumn(
                name: "PromocionId",
                table: "Producto",
                newName: "IdPromocion");
        }
    }
}
