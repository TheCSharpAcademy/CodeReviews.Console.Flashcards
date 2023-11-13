CREATE PROCEDURE [dbo].[Flashcard_MoveStack_tr]
	@FlashcardId int,
	@StackId int
AS
	BEGIN
		SET NOCOUNT ON;

		UPDATE Flashcard SET StackId = @StackId WHERE FlashcardId = @FlashcardId;
	END
RETURN 0
