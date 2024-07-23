CREATE PROCEDURE [dbo].[GetStack]
	
	@Id	INT

AS
BEGIN

	SELECT TOP 1
		*
	FROM
		[dbo].[Stack]
	WHERE
		[Id] = @Id

END
GO
