CREATE PROCEDURE [dbo].[Flashcard_IsInStack_tr]
	@StackId int,
	@FlashcardId int
AS
	BEGIN
		SET NOCOUNT ON;

		IF EXISTS (SELECT * FROM Flashcard WHERE StackId = @StackId AND FlashcardId = @FlashcardId)
			SELECT 1 AS Result;
		ELSE
			SELECT 0 AS Result;
	END
RETURN
