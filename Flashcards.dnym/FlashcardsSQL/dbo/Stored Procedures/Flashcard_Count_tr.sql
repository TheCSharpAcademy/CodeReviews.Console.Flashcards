CREATE PROCEDURE [dbo].[Flashcard_Count_tr]
    @StackId INT = NULL
AS
	BEGIN
		SET NOCOUNT ON;

		SELECT
			COUNT(F.FlashcardId) as Flashcards
		FROM Flashcard AS F
		WHERE F.StackId = @StackId;
	END
RETURN 0
