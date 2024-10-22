-- SELECT * FROM flashcards;

-- DELETE FROM flashcards;
-- DELETE FROM stacks;
-- DELETE FROM study_sessions;
-- DBCC CHECKIDENT ('stacks', RESEED, 0);
-- DBCC CHECKIDENT ('flashcards', RESEED, 0);
-- DBCC CHECKIDENT ('study_sessions', RESEED, 0);

-- EXEC sp_help 'flashcards';
-- EXEC sp_rename 'flashcards.StackId', 'stackID', 'COLUMN';

-- UPDATE study_sessions SET questions = 10 WHERE Id = 2;

-- ALTER TABLE study_sessions
-- ALTER COLUMN score INT;

-- ALTER TABLE study_sessions
-- ADD questions INT;


-- ALTER TABLE flashcards 
-- ALTER TABLE flashcards
-- ALTER COLUMN StackId stackID;

-- ADD Id INT PRIMARY KEY IDENTITY(1,1);



-- USE master;
-- GO

-- ALTER DATABASE FlashCards SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
-- GO
-- DROP DATABASE FlashCards;
-- GO

-- SELECT 
--     stacks.Id,
--     stacks.name,
--     COUNT(study_sessions.Id) AS session_count
-- FROM 
--     stacks
-- LEFT JOIN 
--     study_sessions ON stacks.Id = study_sessions.stackId
-- GROUP BY 
--     stacks.Id, stacks.name;
