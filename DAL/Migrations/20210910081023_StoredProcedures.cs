using DAL.Properties;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class StoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(Resources.ChangePrice);

            migrationBuilder.Sql(@"CREATE PROCEDURE GetOrderSummary
                @OrderId int
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SELECT o.Id, o.[DateTime], SUM(p.Price) AS Price
                    FROM dbo.[Order] o
                    JOIN Product p ON p.OrderId = o.Id
                    WHERE @OrderId = o.Id
					GROUP BY o.Id, o.[DateTime]
                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE GetOrderSummary");
            migrationBuilder.Sql("DROP PROCEDURE ChangePrice");
        }
    }
}
