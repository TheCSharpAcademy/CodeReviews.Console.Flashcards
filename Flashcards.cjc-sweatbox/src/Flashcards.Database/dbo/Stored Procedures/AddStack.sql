CREATE PROCEDURE [dbo].[AddStack]

	@Name NVARCHAR(64)

AS
BEGIN

	INSERT INTO [dbo].[Stack] 
	(
		[Name]
	)
	VALUES
	(
		@Name
	)

END
GO
