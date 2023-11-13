CREATE PROCEDURE [dbo].[Flashcard_Create_tr]
	@StackId int,
	@Front nvarchar(MAX),
	@Back nvarchar(MAX)
AS
	BEGIN
		SET NOCOUNT ON;
		INSERT INTO [dbo].[Flashcard] ([StackId], [Front], [Back])
		VALUES (@StackId, @Front, @Back);
	END
RETURN 0
