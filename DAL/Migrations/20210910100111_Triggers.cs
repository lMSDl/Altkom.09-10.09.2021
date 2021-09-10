using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Triggers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastEdited",
                table: "Product",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.Sql(@"
CREATE TRIGGER Product_UPDATE ON Product
    AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Id int

    SELECT @Id = INSERTED.Id
    FROM INSERTED
    
    UPDATE Product
    SET LastEdited = GETDATE()
    WHERE @Id = Id
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastEdited",
                table: "Product");
        }
    }
}
