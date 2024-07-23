CREATE PROCEDURE [dbo].[SetFlashcard]

	 @Id		INT
	,@Question	NVARCHAR(256)
	,@Answer	NVARCHAR(256)

AS
BEGIN

	UPDATE
		[dbo].[Flashcard] 
	SET
		 [Question] = @Question
		,[Answer] = @Answer
	WHERE
		[Id] = @Id

END
GO
