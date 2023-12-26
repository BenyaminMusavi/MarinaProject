using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marina.UI.Migrations
{
    /// <inheritdoc />
    public partial class notImported : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 24, 15, 37, 16, 576, DateTimeKind.Local).AddTicks(2921),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 12, 24, 10, 20, 2, 772, DateTimeKind.Local).AddTicks(4823));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2023, 12, 24, 15, 37, 16, 576, DateTimeKind.Local).AddTicks(3449));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 24, 10, 20, 2, 772, DateTimeKind.Local).AddTicks(4823),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 12, 24, 15, 37, 16, 576, DateTimeKind.Local).AddTicks(2921));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2023, 12, 24, 10, 20, 2, 772, DateTimeKind.Local).AddTicks(5618));
        }
    }
}
