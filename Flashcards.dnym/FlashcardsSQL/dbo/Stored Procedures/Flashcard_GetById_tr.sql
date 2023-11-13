CREATE PROCEDURE [dbo].[Flashcard_GetById_tr]
	@FlashcardId int
AS
	BEGIN
		SET NOCOUNT ON;

		SELECT TOP 1
			F.FlashcardId, F.Front, F.Back
		FROM Flashcard AS F
		WHERE F.FlashcardId = @FlashcardId;
	END
RETURN 0
