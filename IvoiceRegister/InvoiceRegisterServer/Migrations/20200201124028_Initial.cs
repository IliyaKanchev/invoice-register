using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InvoiceRegisterServer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Salt = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Sum = table.Column<double>(nullable: false),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Client1" });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Client2" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Password", "Salt", "Username" },
                values: new object[] { 1, "HRk1GD0OjiuqVO192KZgdRtltmrWVFn5Oub5Z2BumJs=", "quuuU4fwrLI+fHAAABREBw==", "root" });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "Id", "ClientId", "Date", "Description", "Number", "Sum" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2020, 2, 1, 14, 40, 27, 854, DateTimeKind.Local).AddTicks(6297), "description 1", 1, 100.0 },
                    { 2, 1, new DateTime(2020, 2, 1, 14, 40, 27, 855, DateTimeKind.Local).AddTicks(1096), "description 2", 2, 200.0 },
                    { 3, 2, new DateTime(2020, 2, 1, 14, 40, 27, 855, DateTimeKind.Local).AddTicks(1122), "description 3", 3, 300.0 },
                    { 4, 2, new DateTime(2020, 2, 1, 14, 40, 27, 855, DateTimeKind.Local).AddTicks(1124), "description 4", 4, 400.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ClientId",
                table: "Invoices",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
