-- Thanks again ChatGPT! 
DECLARE @StackId INT;
DECLARE @Score INT;
DECLARE @Questions INT;
DECLARE @Month INT;

SET @Month = 1;

WHILE @Month <= 9
BEGIN
    DECLARE @SessionCount INT = 1;
    
    WHILE @SessionCount <= 3
    BEGIN
        SET @Questions = CAST(RAND() * 10 + 5 AS INT);
        SET @Score = CAST(RAND() * @Questions AS INT);
        INSERT INTO study_sessions (date, score, questions, stackId)
        VALUES 
        (DATEADD(MONTH, @Month - 1, '2024-01-01') + CAST(RAND() * 30 AS INT), @Score, @Questions, CAST(RAND() * 3 + 1 AS INT));

        SET @SessionCount = @SessionCount + 1;
    END

    SET @Month = @Month + 1;
END
