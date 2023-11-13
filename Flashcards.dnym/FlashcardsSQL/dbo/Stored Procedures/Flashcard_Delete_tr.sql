CREATE PROCEDURE [dbo].[Flashcard_Delete_tr]
	@FlashcardId int
AS
	BEGIN
		SET NOCOUNT ON;

		DELETE FROM Flashcard WHERE FlashcardId = @FlashcardId;
	END
RETURN 0
