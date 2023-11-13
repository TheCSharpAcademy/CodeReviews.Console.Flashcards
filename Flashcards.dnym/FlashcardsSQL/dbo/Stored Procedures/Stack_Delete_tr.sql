CREATE PROCEDURE [dbo].[Stack_Delete_tr]
	@StackId int
AS
	BEGIN
		SET NOCOUNT ON;

		DELETE FROM [dbo].[Stack]
		WHERE [StackId] = @StackId;
	END
RETURN 0
