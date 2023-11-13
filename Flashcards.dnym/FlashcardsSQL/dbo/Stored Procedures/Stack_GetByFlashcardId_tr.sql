CREATE PROCEDURE [dbo].[Stack_GetByFlashcardId_tr]
	@FlashcardId int
AS
	BEGIN
		SET NOCOUNT ON;

		SELECT TOP 1
			S.StackId, S.ViewName,
			(SELECT COUNT(1) FROM Flashcard AS FC WHERE FC.StackId = S.StackId) as Cards,
			MAX(H.StartedAt) as LastStudied
		FROM Stack AS S
		LEFT JOIN Flashcard AS F ON S.StackId = F.StackId
		LEFT JOIN History AS H ON S.StackId = H.StackId
		WHERE F.FlashcardId = @FlashcardId
		GROUP BY S.StackId, S.ViewName;
	END
RETURN 0
