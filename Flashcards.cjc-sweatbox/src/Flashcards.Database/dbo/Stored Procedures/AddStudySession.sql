CREATE PROCEDURE [dbo].[AddStudySession]

	 @StackId	INT
	,@DateTime	DATETIME
	,@Score		INT

AS
BEGIN

	INSERT INTO [dbo].[StudySession]
	(
		 [StackId]
		,[DateTime]
		,[Score]
	)
	VALUES
	(
		 @StackId
		,@DateTime
		,@Score
	)

END
GO
