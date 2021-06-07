using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Migration003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommonCodeStatusId",
                table: "Shipments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommonCodeStatusId",
                table: "FixOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_CommonCodeStatusId",
                table: "Shipments",
                column: "CommonCodeStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FixOrders_CommonCodeStatusId",
                table: "FixOrders",
                column: "CommonCodeStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_FixOrders_CommonCodes_CommonCodeStatusId",
                table: "FixOrders",
                column: "CommonCodeStatusId",
                principalTable: "CommonCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_CommonCodes_CommonCodeStatusId",
                table: "Shipments",
                column: "CommonCodeStatusId",
                principalTable: "CommonCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FixOrders_CommonCodes_CommonCodeStatusId",
                table: "FixOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_CommonCodes_CommonCodeStatusId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_CommonCodeStatusId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_FixOrders_CommonCodeStatusId",
                table: "FixOrders");

            migrationBuilder.DropColumn(
                name: "CommonCodeStatusId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "CommonCodeStatusId",
                table: "FixOrders");
        }
    }
}
