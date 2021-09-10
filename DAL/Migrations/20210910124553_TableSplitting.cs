using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class TableSplitting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "From",
                table: "Person",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "In",
                table: "AddressInTown",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "In",
                table: "AddressInCity",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "From",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "In",
                table: "AddressInTown");

            migrationBuilder.DropColumn(
                name: "In",
                table: "AddressInCity");

            migrationBuilder.DropColumn(
                name: "Location_Latitude",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "Location_Longitude",
                table: "Address");
        }
    }
}
