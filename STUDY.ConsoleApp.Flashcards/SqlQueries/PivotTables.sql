
SELECT stack_name ,
	[1] as January,
	[2] as February,
	[3] as March, 
	[4] as April, 
	[5] as May,
	[6] as June,
	[7] as July,
	[8] as August, 
	[9] as September, 
	[10] as October,
	[11] as November,
	[12] as December

FROM 
(
	SELECT Month(session_date) as MonthNumber, stack_name, score
	FROM study_sessions as ss
	INNER JOIN stack as st ON ss.stack_id = st.stack_id
	WHERE YEAR(session_date) = 2025
) AS source
PIVOT
(
	Count(score) FOR MonthNumber IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10] ,[11], [12])
) as pt;


SELECT stack_name ,
	[1] as January,
	[2] as February,
	[3] as March, 
	[4] as April, 
	[5] as May,
	[6] as June,
	[7] as July,
	[8] as August, 
	[9] as September, 
	[10] as October,
	[11] as November,
	[12] as December

FROM 
(
	SELECT Month(session_date) as MonthNumber, stack_name, score
	FROM study_sessions as ss
	INNER JOIN stack as st ON ss.stack_id = st.stack_id
	WHERE YEAR(session_date) = 2025
) AS source
PIVOT
(
	AVG(score) FOR MonthNumber IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10] ,[11], [12])
) as pt;