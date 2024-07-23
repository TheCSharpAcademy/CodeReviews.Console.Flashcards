CREATE PROCEDURE [dbo].[GetStackByName]
	
	@Name	NVARCHAR(64)

AS
BEGIN

	SELECT
		*
	FROM
		[dbo].[Stack]
	WHERE
		[Id] = [dbo].[fnGetStackId](@Name)

END
GO
