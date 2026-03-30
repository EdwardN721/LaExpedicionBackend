using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaExpedicion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarProgresionPersonaje : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Dinero",
                table: "Personajes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Experiencia",
                table: "Personajes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Nivel",
                table: "Personajes",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "SaludActual",
                table: "Personajes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dinero",
                table: "Personajes");

            migrationBuilder.DropColumn(
                name: "Experiencia",
                table: "Personajes");

            migrationBuilder.DropColumn(
                name: "Nivel",
                table: "Personajes");

            migrationBuilder.DropColumn(
                name: "SaludActual",
                table: "Personajes");
        }
    }
}
