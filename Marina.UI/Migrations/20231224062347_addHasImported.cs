using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marina.UI.Migrations
{
    /// <inheritdoc />
    public partial class addHasImported : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Supervisor_SupervisorId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_SupervisorId",
                table: "User");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 24, 9, 53, 46, 587, DateTimeKind.Local).AddTicks(4797),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 12, 16, 0, 42, 11, 972, DateTimeKind.Local).AddTicks(2472));

            migrationBuilder.AddColumn<bool>(
                name: "HasImported",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Supervisor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SupervisorUser",
                columns: table => new
                {
                    SupervisorId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupervisorUser", x => new { x.SupervisorId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_SupervisorUser_Supervisor_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "Supervisor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupervisorUser_User_UsersId",
                        column: x => x.UsersId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Supervisor",
                keyColumn: "Id",
                keyValue: 1,
                column: "Email",
                value: "beni97d@gmail.com");

            migrationBuilder.UpdateData(
                table: "Supervisor",
                keyColumn: "Id",
                keyValue: 2,
                column: "Email",
                value: "beni97d@gmail.com");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "HasImported" },
                values: new object[] { new DateTime(2023, 12, 24, 9, 53, 46, 587, DateTimeKind.Local).AddTicks(5830), false });

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorUser_UsersId",
                table: "SupervisorUser",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupervisorUser");

            migrationBuilder.DropColumn(
                name: "HasImported",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Supervisor");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 16, 0, 42, 11, 972, DateTimeKind.Local).AddTicks(2472),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 12, 24, 9, 53, 46, 587, DateTimeKind.Local).AddTicks(4797));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2023, 12, 16, 0, 42, 11, 972, DateTimeKind.Local).AddTicks(3193));

            migrationBuilder.CreateIndex(
                name: "IX_User_SupervisorId",
                table: "User",
                column: "SupervisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Supervisor_SupervisorId",
                table: "User",
                column: "SupervisorId",
                principalTable: "Supervisor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
