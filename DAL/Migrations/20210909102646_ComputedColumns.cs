using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ComputedColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "[Category] + ': ' + [Name]",
                stored: true);

            migrationBuilder.AddColumn<int>(
                name: "DaysFromOrder",
                table: "Order",
                type: "int",
                nullable: false,
                computedColumnSql: "DATEDIFF(SECOND,[DateTime],GETDATE())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DaysFromOrder",
                table: "Order");
        }
    }
}
