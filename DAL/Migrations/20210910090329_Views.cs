using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Views : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE VIEW View_OrderSummary AS
SELECT o.Id, o.[DateTime], SUM(p.Price) AS Price
FROM dbo.[Order] o
JOIN Product p ON p.OrderId = o.Id
GROUP BY o.Id, o.[DateTime]
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW View_OrderSummary");
        }
    }
}
