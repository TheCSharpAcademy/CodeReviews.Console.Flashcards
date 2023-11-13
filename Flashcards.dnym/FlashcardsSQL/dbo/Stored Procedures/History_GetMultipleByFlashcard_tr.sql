CREATE PROCEDURE [dbo].[History_GetMultipleByFlashcard_tr]
	@FlashcardId int
AS
	BEGIN
		SET NOCOUNT ON;

		SELECT H.HistoryId, H.StackId, H.StartedAt FROM History H
		INNER JOIN StudyResult SR ON H.HistoryId = SR.HistoryId
		WHERE SR.FlashcardId = @FlashcardId;
	END
RETURN 0
