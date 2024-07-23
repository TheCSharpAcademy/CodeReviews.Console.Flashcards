CREATE PROCEDURE [dbo].[DeleteStack]
	
	@Id	INT

AS
BEGIN

	DELETE FROM
		[dbo].[Stack]
	WHERE
		[Id] = @Id

END
GO
