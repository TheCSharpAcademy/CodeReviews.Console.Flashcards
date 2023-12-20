
USE FlashCardsProgram;
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='stacks' and xtype='U')
    CREATE TABLE dbo.stacks (
    stackid INT IDENTITY(1,1) PRIMARY KEY,
    stackname VARCHAR(50) UNIQUE NOT NULL,
    )
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='flashcards' and xtype='U')
    CREATE TABLE dbo.flashcards (
    cardid INT IDENTITY(1,1) PRIMARY KEY,
    stackid INT FOREIGN KEY REFERENCES stacks(stackid) ON DELETE CASCADE ON UPDATE CASCADE,
    question VARCHAR(50) NOT NULL,
    answer VARCHAR(50) NOT NULL,
    )
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='studysessions' and xtype='U')
    CREATE TABLE dbo.studysessions (
    studysessionid INT IDENTITY(1,1) PRIMARY KEY,
    stackname VARCHAR(50) FOREIGN KEY REFERENCES stacks(stackname) ON DELETE CASCADE ON UPDATE CASCADE,
    studysessiondate DATETIME,
    score FLOAT,
    )
GO

SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'
GO

IF EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'stacks')
SELECT 1
ELSE SELECT 0
GO

SELECT stackid, stackname FROM stacks
GO

SELECT * FROM dbo.flashcards
GO

SELECT * FROM dbo.studysessions
GO
