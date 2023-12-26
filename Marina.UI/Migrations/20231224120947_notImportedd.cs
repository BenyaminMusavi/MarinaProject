using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marina.UI.Migrations
{
    /// <inheritdoc />
    public partial class notImportedd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 24, 15, 39, 47, 172, DateTimeKind.Local).AddTicks(7083),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 12, 24, 15, 37, 16, 576, DateTimeKind.Local).AddTicks(2921));

            migrationBuilder.CreateTable(
                name: "NotImportedData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupervisorId = table.Column<int>(type: "int", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 12, 24, 15, 39, 47, 172, DateTimeKind.Local).AddTicks(2499))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotImportedData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotImportedData_Supervisor_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "Supervisor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2023, 12, 24, 15, 39, 47, 172, DateTimeKind.Local).AddTicks(7594));

            migrationBuilder.CreateIndex(
                name: "IX_NotImportedData_SupervisorId",
                table: "NotImportedData",
                column: "SupervisorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotImportedData");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 24, 15, 37, 16, 576, DateTimeKind.Local).AddTicks(2921),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 12, 24, 15, 39, 47, 172, DateTimeKind.Local).AddTicks(7083));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2023, 12, 24, 15, 37, 16, 576, DateTimeKind.Local).AddTicks(3449));
        }
    }
}
