using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaExpedicion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarPrecioAItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Precio",
                table: "Items",
                type: "float",
                nullable: false,
                defaultValue: 50.0);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("0c336332-db1e-4308-a678-e5309c42a795"),
                column: "Precio",
                value: 10000.0);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("1e2dfb2e-9264-4d8f-8a7e-b9a174bb1192"),
                column: "Precio",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("7ea6bd92-05ba-4dea-899d-408e95d28031"),
                column: "Precio",
                value: 50000.0);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("aaef9db3-a67f-46f8-aec7-e37355d454e2"),
                column: "Precio",
                value: 25000.0);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("b5e4edfa-3c3f-4a77-83e7-b34498148df3"),
                column: "Precio",
                value: 1000.0);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("ef70e69f-798f-4923-a8a2-3d88d719eb0f"),
                column: "Precio",
                value: 12000.0);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("f19dd3d1-3837-4a9a-80d1-190a8f128d9c"),
                column: "Precio",
                value: 50.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Precio",
                table: "Items");
        }
    }
}
