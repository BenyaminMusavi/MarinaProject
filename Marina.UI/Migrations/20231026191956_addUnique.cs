using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marina.UI.Migrations
{
    /// <inheritdoc />
    public partial class addUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 26, 22, 49, 56, 105, DateTimeKind.Local).AddTicks(1350),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 10, 26, 17, 57, 19, 38, DateTimeKind.Local).AddTicks(5233));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 26, 17, 57, 19, 38, DateTimeKind.Local).AddTicks(5233),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 10, 26, 22, 49, 56, 105, DateTimeKind.Local).AddTicks(1350));
        }
    }
}
