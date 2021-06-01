using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Migration002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Elements_CommonCodes_CommonCodeCurrencyId",
                table: "Elements");

            migrationBuilder.DropForeignKey(
                name: "FK_Fixs_Customers_CustomerId",
                table: "Fixs");

            migrationBuilder.DropForeignKey(
                name: "FK_Fixs_Locations_Locationid",
                table: "Fixs");

            migrationBuilder.DropIndex(
                name: "IX_Fixs_Locationid",
                table: "Fixs");

            migrationBuilder.DropIndex(
                name: "IX_Elements_CommonCodeCurrencyId",
                table: "Elements");

            migrationBuilder.DropColumn(
                name: "CustsomerId",
                table: "Fixs");

            migrationBuilder.DropColumn(
                name: "Locationid",
                table: "Fixs");

            migrationBuilder.DropColumn(
                name: "CommonCodeCurrencyId",
                table: "Elements");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateStart",
                table: "Shipments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateEnd",
                table: "Shipments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Fixs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrencyRates",
                table: "Fixs",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Fixs_Customers_CustomerId",
                table: "Fixs",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fixs_Customers_CustomerId",
                table: "Fixs");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "CurrencyRates",
                table: "Fixs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateStart",
                table: "Shipments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateEnd",
                table: "Shipments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Fixs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CustsomerId",
                table: "Fixs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Locationid",
                table: "Fixs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommonCodeCurrencyId",
                table: "Elements",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fixs_Locationid",
                table: "Fixs",
                column: "Locationid");

            migrationBuilder.CreateIndex(
                name: "IX_Elements_CommonCodeCurrencyId",
                table: "Elements",
                column: "CommonCodeCurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Elements_CommonCodes_CommonCodeCurrencyId",
                table: "Elements",
                column: "CommonCodeCurrencyId",
                principalTable: "CommonCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fixs_Customers_CustomerId",
                table: "Fixs",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fixs_Locations_Locationid",
                table: "Fixs",
                column: "Locationid",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
