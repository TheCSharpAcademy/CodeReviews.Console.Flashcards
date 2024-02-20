USE master
GO
DROP DATABASE FlashCardsProgram
GO

-- USE master
-- GO
-- CREATE DATABASE FlashCardsProgram
-- GO

-- USE FlashCardsProgram;
-- GO

-- IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='stacks' and xtype='U')
--     CREATE TABLE dbo.stacks (
--     stackid INT IDENTITY(1,1) UNIQUE NOT NULL, 
--     stackname VARCHAR(50) PRIMARY KEY NOT NULL,
--     )
-- GO

-- IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='flashcards' and xtype='U')
--     CREATE TABLE dbo.flashcards (
--     cardid INT IDENTITY(1,1) PRIMARY KEY,
--     stackname VARCHAR(50) FOREIGN KEY REFERENCES stacks(stackname) ON DELETE CASCADE ON UPDATE CASCADE,
--     question NVARCHAR(600) NOT NULL,
--     answer NVARCHAR(600) NOT NULL,
--     )
-- GO

-- IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='studysessions' and xtype='U')
--     CREATE TABLE dbo.studysessions (
--     studysessionid INT IDENTITY(1,1) PRIMARY KEY,
--     stackname VARCHAR(50) FOREIGN KEY REFERENCES stacks(stackname) ON DELETE CASCADE ON UPDATE CASCADE,
--     studysessiondate DATETIME,
--     score FLOAT,
--     )
-- GO

-- SELECT * FROM dbo.stacks
-- GO

-- SELECT * FROM dbo.flashcards
-- GO

-- SELECT * FROM dbo.studysessions
-- GO

-- --  INSERT INTO studysessions
-- --  (
-- --   stackname, studysessiondate, score
-- --  )
-- --  VALUES
-- --  ('spanish', DATETIMEFROMPARTS(2022,10,15,0,0,0,0), 0.25)
-- --  GO

-- SELECT YEAR(studysessiondate) AS 'year', MONTH(studysessiondate) AS 'month', stackname,
-- COUNT(studysessionid) AS 'studysessionqty', AVG(score) AS 'avgscore'
-- FROM studysessions
-- GROUP BY stackName, YEAR(studysessiondate), MONTH(studysessiondate)
-- ORDER BY 'year', 'month', stackName
