using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marina.UI.Migrations
{
    /// <inheritdoc />
    public partial class defualtFalse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 24, 10, 20, 2, 772, DateTimeKind.Local).AddTicks(4823),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 12, 24, 9, 53, 46, 587, DateTimeKind.Local).AddTicks(4797));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2023, 12, 24, 10, 20, 2, 772, DateTimeKind.Local).AddTicks(5618));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 24, 9, 53, 46, 587, DateTimeKind.Local).AddTicks(4797),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 12, 24, 10, 20, 2, 772, DateTimeKind.Local).AddTicks(4823));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2023, 12, 24, 9, 53, 46, 587, DateTimeKind.Local).AddTicks(5830));
        }
    }
}
