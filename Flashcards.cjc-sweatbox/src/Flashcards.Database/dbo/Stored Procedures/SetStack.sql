CREATE PROCEDURE [dbo].[SetStack]

	 @Id	INT
	,@Name	NVARCHAR(64)

AS
BEGIN

	UPDATE 
		[dbo].[Stack]
	SET
		[Name] =  @Name
	WHERE
		[Id] = @Id
	
END
GO
