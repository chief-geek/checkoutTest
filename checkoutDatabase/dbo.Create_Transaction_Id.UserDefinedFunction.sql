CREATE FUNCTION [dbo].[Create_Transaction_Id]
(
	@Amount decimal(18,2),
	@CardId uniqueidentifier
)
RETURNS nvarchar(100)
AS
BEGIN
	DECLARE @Result nvarchar(100)
	set @Result = (SELECT CONCAT(CAST(@Amount AS nvarchar(100)), SUBSTRING(CAST(@CardId as nvarchar(100)), 1, 10)))
	RETURN @Result

END
GO
