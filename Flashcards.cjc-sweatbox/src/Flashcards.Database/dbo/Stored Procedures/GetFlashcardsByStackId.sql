CREATE PROCEDURE [dbo].[GetFlashcardsByStackId]

	@StackId	INT

AS
BEGIN

	SELECT
		*
	FROM 
		[dbo].[Flashcard] 
	WHERE
		[StackId] = @StackId

END
GO
