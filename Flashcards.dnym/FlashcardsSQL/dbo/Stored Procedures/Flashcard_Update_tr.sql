CREATE PROCEDURE [dbo].[Flashcard_Update_tr]
	@FlashcardId int,
	@Front nvarchar(255),
	@Back nvarchar(255)
AS
	BEGIN
		SET NOCOUNT ON;

		UPDATE Flashcard SET Front = @Front, Back = @Back WHERE FlashcardId = @FlashcardId;
	END
RETURN 0
