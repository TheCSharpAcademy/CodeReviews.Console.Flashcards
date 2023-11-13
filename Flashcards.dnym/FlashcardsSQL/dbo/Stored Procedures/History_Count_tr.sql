CREATE PROCEDURE [dbo].[History_Count_tr]
AS
	BEGIN
		SET NOCOUNT ON;

		SELECT
			COUNT(H.HistoryId) as HistorySessions
		FROM History AS H;
	END
RETURN 0
