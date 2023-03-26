using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project_API.Migrations
{
    /// <inheritdoc />
    public partial class updtablevilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "DateCreation", "DateUpdate", "Detail", "Dimension", "ImagenUrl", "Name", "Occupants", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", new DateTime(2023, 3, 26, 10, 3, 15, 456, DateTimeKind.Local).AddTicks(8366), new DateTime(2023, 3, 26, 10, 3, 15, 456, DateTimeKind.Local).AddTicks(8383), "Detalle de la villa...", 50, "", "Villa Real", 5, 200.0 },
                    { 2, "", new DateTime(2023, 3, 26, 10, 3, 15, 456, DateTimeKind.Local).AddTicks(8387), new DateTime(2023, 3, 26, 10, 3, 15, 456, DateTimeKind.Local).AddTicks(8389), "Detalle de la villa Caraz...", 100, "", "Villa Caraz", 10, 400.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
