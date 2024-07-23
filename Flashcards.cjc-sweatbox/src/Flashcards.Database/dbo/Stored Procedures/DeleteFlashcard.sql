CREATE PROCEDURE [dbo].[DeleteFlashcard]

	@Id	INT

AS
BEGIN

	DELETE FROM 
		[dbo].[Flashcard] 
	WHERE
		[Id] = @Id

END
GO
