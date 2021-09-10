using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace DAL.Migrations
{
    public partial class SpatialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location_Latitude",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "Location_Longitude",
                table: "Address");

            migrationBuilder.AddColumn<Point>(
                name: "Location",
                table: "Address",
                type: "geography",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Address");

            migrationBuilder.AddColumn<float>(
                name: "Location_Latitude",
                table: "Address",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Location_Longitude",
                table: "Address",
                type: "real",
                nullable: true);
        }
    }
}
