using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LaExpedicion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarNuevosCatalogosPop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Items",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.InsertData(
                table: "Etiquetas",
                columns: new[] { "Id", "Activo", "Descripcion", "Eliminado", "FechaCreacion", "FechaModificacion", "Nombre", "UsuarioCreacion", "UsuarioModificacion" },
                values: new object[,]
                {
                    { new Guid("0c336332-db1e-4308-a678-e5309c42a795"), false, "Mutante entrenado para rastrear y cazar monstruos a cambio de monedas.", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Brujo", "", "" },
                    { new Guid("2a3dfde7-ce67-4205-8f0f-08f3d6287a0b"), false, "Especialista en tácticas de rescate, armas de fuego y supervivencia extrema.", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Agente S.T.A.R.S.", "", "" },
                    { new Guid("aaef9db3-a67f-46f8-aec7-e37355d454e2"), false, "Guardián de la paz y la justicia, sensible a fuerzas místicas.", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jedi", "", "" },
                    { new Guid("d1111111-1111-1111-1111-111111111111"), false, "Héroe con atributos muy bajos, ¿estará destinado para apuntar a lo grande?", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Novato", "", "" },
                    { new Guid("d2222222-2222-2222-2222-222222222222"), false, "Un combatiente estándar y capaz, listo para forjar su propio destino en este peligroso mundo.", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aventurero", "", "" },
                    { new Guid("d3333333-3333-3333-3333-333333333333"), false, "Destaca entre la multitud. Sus habilidades sobresalientes le auguran un futuro brillante y lleno de victorias.", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Talento Nato", "", "" },
                    { new Guid("d4444444-4444-4444-4444-444444444444"), false, "Héroe imparable, hay muchas expectativas puestas sobre ti. El mundo entero observa tus pasos.", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Genio", "", "" },
                    { new Guid("e8f3cbb0-78a4-4001-85d1-9d923019ed86"), false, "Maestro del sigilo, el parkour y el uso de la hoja oculta bajo las sombras.", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Asesino de la Hermandad", "", "" },
                    { new Guid("f19dd3d1-3837-4a9a-80d1-190a8f128d9c"), false, "Especialista en salir con vida de situaciones biológicas extremas.", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Superviviente", "", "" }
                });

            migrationBuilder.InsertData(
                table: "Expediciones",
                columns: new[] { "Id", "Activo", "Descripcion", "Dinero", "Eliminado", "Experiencia", "FechaCreacion", "FechaModificacion", "Nombre", "UsuarioCreacion", "UsuarioModificacion" },
                values: new object[,]
                {
                    { new Guid("067cdc49-de25-4e88-bec6-42f9aa627dba"), false, "Resiste el asedio del Rey de la Noche y su ejército de caminantes blancos.", 2500.0, false, 3000, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Defensa de Invernalia", "", "" },
                    { new Guid("64fea296-23f9-430e-bec4-f5d01dc86509"), false, "Investiga los extraños incidentes y desapariciones en las oscuras montañas Arklay.", 1500.0, false, 500, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Incidente en la Mansión Spencer", "", "" },
                    { new Guid("65fcf63f-94fb-4f1a-abb6-004a02b078c3"), false, "Abrete paso entre hordas de infectados y mutantes antes de que la ciudad sea purgada del mapa.", 500.0, false, 1500, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Escape de Raccoon City", "", "" },
                    { new Guid("83262a7b-6916-4ec4-b4a6-bb62cbfee247"), false, "Una peligrosa travesía para destruir un objeto de inmenso poder en los fuegos del Monte del Destino.", 0.0, false, 2000, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Viaje a Mordor", "", "" },
                    { new Guid("c3333333-3333-3333-3333-333333333333"), false, "Sobrevive a dragones, rescata rehenes en el lago negro y encuentra la copa en el laberinto.", 10000.0, false, 2500, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Torneo de los Tres Magos", "", "" }
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Activo", "Descripcion", "Eliminado", "FechaCreacion", "FechaModificacion", "Nombre", "UsuarioCreacion", "UsuarioModificacion" },
                values: new object[,]
                {
                    { new Guid("0c336332-db1e-4308-a678-e5309c42a795"), false, "La hoja destructora del mal, forjada por diosas antiguas.", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Espada Maestra", "", "" },
                    { new Guid("1e2dfb2e-9264-4d8f-8a7e-b9a174bb1192"), false, "Cilindro presurizado que restaura la salud por completo de forma casi instantánea.", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spray de Primeros Auxilios", "", "" },
                    { new Guid("7ea6bd92-05ba-4dea-899d-408e95d28031"), false, "Otorga invisibilidad a su portador, pero corrompe su mente lentamente.", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "El Anillo Único", "", "" },
                    { new Guid("aaef9db3-a67f-46f8-aec7-e37355d454e2"), false, "Suerte líquida. Quien la bebe tendrá éxito en todo lo que intente.", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Poción Felix Felicis", "", "" },
                    { new Guid("b5e4edfa-3c3f-4a77-83e7-b34498148df3"), false, "Dispositivo de muñeca que dispara un fluido sintético con la resistencia del acero.", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lanzaredes", "", "" },
                    { new Guid("ef70e69f-798f-4923-a8a2-3d88d719eb0f"), false, "Un arma elegante para una era más civilizada. Capaz de cortar casi cualquier material.", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sable de Luz", "", "" },
                    { new Guid("f19dd3d1-3837-4a9a-80d1-190a8f128d9c"), false, "Planta medicinal clásica que restaura una cantidad moderada de salud. Mejor si se combina.", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hierba Verde", "", "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Etiquetas",
                keyColumn: "Id",
                keyValue: new Guid("0c336332-db1e-4308-a678-e5309c42a795"));

            migrationBuilder.DeleteData(
                table: "Etiquetas",
                keyColumn: "Id",
                keyValue: new Guid("2a3dfde7-ce67-4205-8f0f-08f3d6287a0b"));

            migrationBuilder.DeleteData(
                table: "Etiquetas",
                keyColumn: "Id",
                keyValue: new Guid("aaef9db3-a67f-46f8-aec7-e37355d454e2"));

            migrationBuilder.DeleteData(
                table: "Etiquetas",
                keyColumn: "Id",
                keyValue: new Guid("d1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Etiquetas",
                keyColumn: "Id",
                keyValue: new Guid("d2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Etiquetas",
                keyColumn: "Id",
                keyValue: new Guid("d3333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Etiquetas",
                keyColumn: "Id",
                keyValue: new Guid("d4444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Etiquetas",
                keyColumn: "Id",
                keyValue: new Guid("e8f3cbb0-78a4-4001-85d1-9d923019ed86"));

            migrationBuilder.DeleteData(
                table: "Etiquetas",
                keyColumn: "Id",
                keyValue: new Guid("f19dd3d1-3837-4a9a-80d1-190a8f128d9c"));

            migrationBuilder.DeleteData(
                table: "Expediciones",
                keyColumn: "Id",
                keyValue: new Guid("067cdc49-de25-4e88-bec6-42f9aa627dba"));

            migrationBuilder.DeleteData(
                table: "Expediciones",
                keyColumn: "Id",
                keyValue: new Guid("64fea296-23f9-430e-bec4-f5d01dc86509"));

            migrationBuilder.DeleteData(
                table: "Expediciones",
                keyColumn: "Id",
                keyValue: new Guid("65fcf63f-94fb-4f1a-abb6-004a02b078c3"));

            migrationBuilder.DeleteData(
                table: "Expediciones",
                keyColumn: "Id",
                keyValue: new Guid("83262a7b-6916-4ec4-b4a6-bb62cbfee247"));

            migrationBuilder.DeleteData(
                table: "Expediciones",
                keyColumn: "Id",
                keyValue: new Guid("c3333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("0c336332-db1e-4308-a678-e5309c42a795"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("1e2dfb2e-9264-4d8f-8a7e-b9a174bb1192"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("7ea6bd92-05ba-4dea-899d-408e95d28031"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("aaef9db3-a67f-46f8-aec7-e37355d454e2"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("b5e4edfa-3c3f-4a77-83e7-b34498148df3"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("ef70e69f-798f-4923-a8a2-3d88d719eb0f"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("f19dd3d1-3837-4a9a-80d1-190a8f128d9c"));

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Items",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
