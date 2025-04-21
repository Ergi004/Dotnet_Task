using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_tut.Migrations
{
    /// <inheritdoc />
    public partial class CreatedCustomerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerFullName",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CustomerMainAddress",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CustomerMainEmailAddress",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CustomerMainPhoneNumber",
                table: "Transactions");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainEmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CustomerId",
                table: "Transactions",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Customer_CustomerId",
                table: "Transactions",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Customer_CustomerId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CustomerId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "CustomerFullName",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerMainAddress",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerMainEmailAddress",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerMainPhoneNumber",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
