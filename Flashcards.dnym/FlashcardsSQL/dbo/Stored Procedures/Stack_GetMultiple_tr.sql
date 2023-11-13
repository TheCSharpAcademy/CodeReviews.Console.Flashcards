CREATE PROCEDURE [dbo].[Stack_GetMultiple_tr]
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
            S.StackId, S.ViewName,
            COUNT(DISTINCT F.FlashcardId) as Cards,
            MAX(H.StartedAt) as LastStudied
        FROM Stack AS S
        LEFT JOIN Flashcard AS F ON S.StackId = F.StackId
        LEFT JOIN History AS H ON S.StackId = H.StackId
        GROUP BY S.StackId, S.ViewName
        ORDER BY S.StackId
        OFFSET ISNULL(@Skip, @SKIP_FALLBACK) ROWS
        FETCH NEXT ISNULL(@Take, @TAKE_FALLBACK) ROWS ONLY;
    END
RETURN 0
