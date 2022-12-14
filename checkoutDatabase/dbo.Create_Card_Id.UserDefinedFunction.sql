CREATE FUNCTION [dbo].[Create_Card_Id]
(
	@CardNumber decimal(18,2)
)
RETURNS nvarchar(100)
AS
	BEGIN
		DECLARE @Result nvarchar(100)
		set @Result = (SELECT CONCAT(@CardNumber, SUBSTRING(CAST(GETDATE() as nvarchar(100)), 1, 4)))
		RETURN @Result
	END
GO

