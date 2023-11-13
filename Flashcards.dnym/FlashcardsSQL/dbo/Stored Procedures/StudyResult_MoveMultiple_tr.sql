CREATE PROCEDURE [dbo].[StudyResult_MoveMultiple_tr]
	@FlashcardId int,
	@OldHistoryId int,
	@NewHistoryId int
AS
	BEGIN
		SET NOCOUNT ON;

		UPDATE StudyResult SET HistoryId = @NewHistoryId WHERE FlashcardId = @FlashcardId AND HistoryId = @OldHistoryId;
	END
RETURN 0
