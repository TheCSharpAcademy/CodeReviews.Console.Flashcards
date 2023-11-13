CREATE PROCEDURE [dbo].[History_GetByStackAndDateOrCreate_tr]
	@StackId int,
	@StartedAt datetime2(7)
AS
	BEGIN
		SET NOCOUNT ON;

		DECLARE @HistoryId int;
		DECLARE @HistoryCount int;

		SELECT @HistoryCount = COUNT(1) FROM History WHERE StackId = @StackId AND StartedAt = @StartedAt;

		IF @HistoryCount = 0
			BEGIN
				INSERT INTO History (StackId, StartedAt) VALUES (@StackId, @StartedAt);
				SELECT @HistoryId = SCOPE_IDENTITY();
			END
		ELSE
			BEGIN
				SELECT @HistoryId = HistoryId FROM History WHERE StackId = @StackId AND StartedAt = @StartedAt;
			END

		SELECT @HistoryId AS HistoryId;
	END
RETURN 0
