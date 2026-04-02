using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaExpedicion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTiposDeItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoItem",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("0c336332-db1e-4308-a678-e5309c42a795"),
                column: "TipoItem",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("1e2dfb2e-9264-4d8f-8a7e-b9a174bb1192"),
                column: "TipoItem",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("7ea6bd92-05ba-4dea-899d-408e95d28031"),
                column: "TipoItem",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("aaef9db3-a67f-46f8-aec7-e37355d454e2"),
                column: "TipoItem",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("b5e4edfa-3c3f-4a77-83e7-b34498148df3"),
                column: "TipoItem",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("ef70e69f-798f-4923-a8a2-3d88d719eb0f"),
                column: "TipoItem",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("f19dd3d1-3837-4a9a-80d1-190a8f128d9c"),
                column: "TipoItem",
                value: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoItem",
                table: "Items");
        }
    }
}
