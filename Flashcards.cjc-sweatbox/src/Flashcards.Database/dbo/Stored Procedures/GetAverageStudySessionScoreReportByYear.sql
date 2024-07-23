CREATE PROCEDURE [dbo].[GetAverageStudySessionScoreReportByYear]

	 @Year	NVARCHAR(4)

AS
BEGIN

	SELECT
		*
	FROM
		[dbo].[vwAverageStudySessionScoreReport]
	WHERE
		[StudyYear] = @Year
	ORDER BY
		[StackName]

END
GO
