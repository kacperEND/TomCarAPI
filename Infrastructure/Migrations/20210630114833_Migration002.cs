using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Migration002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalFieldName",
                table: "FixOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AdditionalFieldValue",
                table: "FixOrders",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FixOrders",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalFieldName",
                table: "FixOrders");

            migrationBuilder.DropColumn(
                name: "AdditionalFieldValue",
                table: "FixOrders");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "FixOrders");
        }
    }
}
