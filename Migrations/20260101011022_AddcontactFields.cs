using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BnsBazarApp.Migrations
{
    /// <inheritdoc />
    public partial class AddcontactFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Agency",
                table: "Advertisements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Advertisements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Advertisements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 1, 1, 10, 21, 369, DateTimeKind.Utc).AddTicks(3870), "$2a$11$S6otxunchl3n.jqeel6xR.6xoCl9B.tBUULPnvZSJEIMQUJCFCgie" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Agency",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Advertisements");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 1, 35, 22, 970, DateTimeKind.Utc).AddTicks(1609), "$2a$11$Q6DWOwXllMHyPt2MxRO6seldIa1IvFoaLQbR1F4r3nd5JE2xGrznW" });
        }
    }
}
