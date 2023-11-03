using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Marina.UI.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Distributor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distributor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Line",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Line", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Province",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Province", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RSM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSM", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistributorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    RSMId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DistributorId = table.Column<int>(type: "int", nullable: false),
                    LineId = table.Column<int>(type: "int", nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 11, 3, 11, 5, 20, 552, DateTimeKind.Local).AddTicks(3477)),
                    UpdaterUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Distributor_DistributorId",
                        column: x => x.DistributorId,
                        principalTable: "Distributor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Line_LineId",
                        column: x => x.LineId,
                        principalTable: "Line",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Province",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Distributor",
                columns: new[] { "Id", "Code" },
                values: new object[,]
                {
                    { 1, "D1" },
                    { 2, "D2" },
                    { 3, "D3" }
                });

            migrationBuilder.InsertData(
                table: "Line",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "SunStar" },
                    { 2, "Sunnyness" }
                });

            migrationBuilder.InsertData(
                table: "Province",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Teshran" },
                    { 2, "Shiraz" }
                });

            migrationBuilder.InsertData(
                table: "RSM",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "RSM1" },
                    { 2, "RSM2" },
                    { 3, "RSM3" }
                });

            migrationBuilder.InsertData(
                table: "Region",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "R1" },
                    { 2, "R2" },
                    { 3, "R3" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreateDate", "DistributorId", "DistributorName", "IsActive", "LineId", "PasswordHash", "PhoneNumber", "ProvinceId", "RSMId", "RegionId", "Salt", "UpdateTime", "UpdaterUserId", "UserName" },
                values: new object[] { 1, new DateTime(2023, 11, 3, 11, 5, 20, 552, DateTimeKind.Local).AddTicks(4041), 1, "admin", true, 1, "Gco+uHGl5M4H2AXm7UqdfBz/VrFZrLUQiXy9tU9f9d8=", "0901", 1, 1, 1, "wJ8Ddinmsj1ZLo5+J9N0FvchZRgOeGlRLDKIIZu3KAs=", null, null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_User_DistributorId",
                table: "User",
                column: "DistributorId");

            migrationBuilder.CreateIndex(
                name: "IX_User_LineId",
                table: "User",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_User_ProvinceId",
                table: "User",
                column: "ProvinceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropTable(
                name: "RSM");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Distributor");

            migrationBuilder.DropTable(
                name: "Line");

            migrationBuilder.DropTable(
                name: "Province");
        }
    }
}
