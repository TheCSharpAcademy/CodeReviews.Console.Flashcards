CREATE PROCEDURE [dbo].[History_GetMultiple_tr]
    @Skip INT = NULL,
    @Take INT = NULL
AS
    BEGIN
        SET NOCOUNT ON;

        DECLARE @SKIP_FALLBACK INT;
        SET @SKIP_FALLBACK = 0;
        DECLARE @TAKE_FALLBACK INT;
        SET @TAKE_FALLBACK = 2147483647;

        SELECT
            H.HistoryId, H.StartedAt,
            S.ViewName,
            COUNT(R.FlashcardId) as CardsStudied,
            SUM(CASE WHEN R.WasCorrect = 1 THEN 1 ELSE 0 END) AS CorrectAnswers
        FROM History AS H
        INNER JOIN Stack AS S ON H.StackId = S.StackId
        LEFT JOIN StudyResult AS R ON H.HistoryId = R.HistoryId
        GROUP BY H.HistoryId, H.StartedAt, S.ViewName
        ORDER BY H.HistoryId
        OFFSET ISNULL(@Skip, @SKIP_FALLBACK) ROWS
        FETCH NEXT ISNULL(@Take, @TAKE_FALLBACK) ROWS ONLY;
    END
RETURN 0
