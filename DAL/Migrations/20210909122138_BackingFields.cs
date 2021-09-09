using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class BackingFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "_secret",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true,
                defaultValueSql: "NEWID()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_secret",
                table: "Product");
        }
    }
}
