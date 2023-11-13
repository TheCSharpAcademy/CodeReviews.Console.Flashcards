CREATE PROCEDURE [dbo].[History_DeleteUnused_tr]
	@HistoryId int
AS
	BEGIN
		SET NOCOUNT ON;

		DELETE FROM History
		WHERE HistoryId = @HistoryId
			AND NOT EXISTS (SELECT 1 FROM StudyResult WHERE HistoryId = @HistoryId);
	END
RETURN 0
