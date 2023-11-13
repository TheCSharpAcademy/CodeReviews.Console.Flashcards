CREATE PROCEDURE [dbo].[Stack_Create_tr]
	@ViewName nvarchar(255),
	@SortName nvarchar(255)
AS
	BEGIN
		SET NOCOUNT ON;
		INSERT INTO [dbo].[Stack] ([ViewName], [SortName])
		VALUES (@ViewName, @SortName);
	END
RETURN 0
