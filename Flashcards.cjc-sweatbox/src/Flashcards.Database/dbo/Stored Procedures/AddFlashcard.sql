CREATE PROCEDURE [dbo].[AddFlashcard]

	 @StackId	INT
	,@Question	NVARCHAR(256)
	,@Answer	NVARCHAR(256)

AS
BEGIN

	INSERT INTO [dbo].[Flashcard] 
	(
		 [StackId]
		,[Question]
		,[Answer]
	)
	VALUES
	(
		 @StackId
		,@Question
		,@Answer
	)

END
GO
