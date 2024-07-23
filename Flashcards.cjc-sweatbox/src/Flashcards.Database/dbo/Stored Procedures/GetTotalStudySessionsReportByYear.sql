CREATE PROCEDURE [dbo].[GetTotalStudySessionsReportByYear]

	 @Year	NVARCHAR(4)

AS
BEGIN

	SELECT
		*
	FROM
		[dbo].[vwTotalStudySessionsReport]
	WHERE
		[StudyYear] = @Year
	ORDER BY
		[StackName]

END
GO
