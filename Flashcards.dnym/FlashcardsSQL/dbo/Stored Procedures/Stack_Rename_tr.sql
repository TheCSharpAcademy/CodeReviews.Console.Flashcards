CREATE PROCEDURE [dbo].[Stack_Rename_tr]
	@StackId int,
	@ViewName nvarchar(255),
	@SortName nvarchar(255)
AS
	BEGIN
		SET NOCOUNT ON;

		UPDATE [dbo].[Stack]
		SET [ViewName] = @ViewName, [SortName] = @SortName
		WHERE [StackId] = @StackId;
	END
RETURN 0
