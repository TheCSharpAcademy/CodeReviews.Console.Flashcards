CREATE FUNCTION [dbo].[fnGetStackId]
(
	@Name	NVARCHAR(64)
)
RETURNS INT
AS
BEGIN

	DECLARE @Id INT

	SET @Id =
	(
		SELECT TOP 1
			[Id]
		FROM
			[dbo].[Stack]
		WHERE
			[Name] = @Name
	)

	RETURN @Id

END
GO

