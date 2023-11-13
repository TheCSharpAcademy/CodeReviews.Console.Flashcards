CREATE PROCEDURE [dbo].[StudyResult_Create_tr]
	@HistoryId int,
	@FlashcardId int,
	@WasCorrect bit,
	@AnsweredAt datetime2(7)
AS
	BEGIN
		SET NOCOUNT ON;
		INSERT INTO [dbo].[StudyResult] ([HistoryId], [FlashcardId], [WasCorrect], [AnsweredAt])
		VALUES (@HistoryId, @FlashcardId, @WasCorrect, @AnsweredAt);
	END
RETURN 0
