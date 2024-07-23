CREATE PROCEDURE [dbo].[GetFlashcard]

	@Id	INT

AS
BEGIN

	SELECT TOP 1
		*
	FROM 
		[dbo].[Flashcard] 
	WHERE
		[Id] = @Id

END
GO
