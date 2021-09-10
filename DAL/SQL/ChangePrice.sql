CREATE OR ALTER PROCEDURE ChangePrice
@MinimalPrice int
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Product SET Price = Price + 1
    WHERE @MinimalPrice >= Price
END