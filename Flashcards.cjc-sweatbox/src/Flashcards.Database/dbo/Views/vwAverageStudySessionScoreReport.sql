CREATE VIEW [dbo].[vwAverageStudySessionScoreReport] 
AS
	SELECT
		 [StackName]
		,[StudyYear]
		,COALESCE([January], 0) AS [January]
		,COALESCE([February], 0) AS [February]
		,COALESCE([March], 0) AS [March]
		,COALESCE([April], 0) AS [April]
		,COALESCE([May], 0) AS [May]
		,COALESCE([June], 0) AS [June]
		,COALESCE([July], 0) AS [July]
		,COALESCE([August], 0) AS [August]
		,COALESCE([September], 0) AS [September]
		,COALESCE([October], 0) AS [October]
		,COALESCE([November], 0) AS [November]
		,COALESCE([December], 0) AS [December]
	FROM
	(
		SELECT
			 st.[Name] AS [StackName]
			,DATENAME(year, ss.[DateTime]) AS [StudyYear]
			,DATENAME(month, ss.[DateTime]) AS [StudyMonth]
			,ss.[Score]
		FROM
			[dbo].[Stack]			AS st JOIN
			[dbo].[StudySession]	AS ss ON st.[Id] = ss.[StackId]
	) AS s
	PIVOT
	(
		AVG([Score])
		FOR [StudyMonth] IN ([January], [February], [March], [April], [May], [June], [July], [August], [September], [October], [November], [December])
	) AS p
GO
