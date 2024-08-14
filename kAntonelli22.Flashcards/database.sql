-- USE master

-- IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'FlashcardDB')
--     CREATE DATABASE [FlashcardDB]

USE FlashcardDB;

GO

-- IF OBJECT_ID(N'dbo.Stacks', N'U') IS NULL
-- CREATE TABLE dbo.Stacks (
--     Id INT NOT NULL PRIMARY KEY IDENTITY,
--     StackName NVARCHAR(255) NOT NULL,
--     StackSize INT NOT NULL
-- )

-- GO

-- IF OBJECT_ID(N'dbo.Cards', N'U') IS NULL
-- CREATE TABLE dbo.Cards (
--     Id INT NOT NULL PRIMARY KEY IDENTITY,
--     Front NVARCHAR(255) NOT NULL,
--     Back NVARCHAR(255) NOT NULL,
--     Stack_Id INT NOT NULL,
--     FOREIGN KEY(Stack_Id) REFERENCES dbo.Stacks(Id)
-- )

-- GO

-- IF OBJECT_ID(N'dbo.Sessions', N'U') IS NULL
-- CREATE TABLE dbo.Sessions (
--     Id INT NOT NULL PRIMARY KEY IDENTITY,
--     StackName NVARCHAR(255) NOT NULL,
--     StackSize INT NOT NULL,
--     Stack_Id INT NOT NULL,
--     NumComplete INT NOT NULL,
--     NumCorrect INT NOT NULL,
--     AvgTime FLOAT NOT NULL,
--     Date NVARCHAR(255) NOT NULL
--     FOREIGN KEY(Stack_Id) REFERENCES dbo.Stacks(Id)
-- )

-- GO

-- DELETE FROM dbo.Cards WHERE Stack_Id = '2'
-- DELETE FROM dbo.Stacks WHERE StackName = 'Stack 2'

-- GO

-- UPDATE dbo.Stacks 
--     SET StackName = 'Stack 5'
--     WHERE StackName = 'Stack 4'

-- GO

DROP TABLE dbo.Cards;
DROP TABLE dbo.Sessions;
DROP TABLE dbo.Stacks;

GO

-- UPDATE dbo.Cards SET Front = '{currentCard.Front}', Back = '{currentCard.Back}' WHERE Front = 'card.Front';
-- SELECT * FROM dbo.Cards
-- GO


USE master; IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'FlashcardDB')
    CREATE DATABASE [FlashcardDB];
GO

USE FlashcardDB; 
GO

IF OBJECT_ID(N'dbo.Stacks', N'U') IS NULL
CREATE TABLE dbo.Stacks (Id INT NOT NULL PRIMARY KEY IDENTITY, StackName NVARCHAR(255) NOT NULL, StackSize INT NOT NULL);

IF OBJECT_ID(N'dbo.Cards', N'U') IS NULL
CREATE TABLE dbo.Cards (Id INT NOT NULL PRIMARY KEY IDENTITY, Front NVARCHAR(255) NOT NULL, Back NVARCHAR(255) NOT NULL,
    Stack_Id INT NOT NULL, FOREIGN KEY(Stack_Id) REFERENCES dbo.Stacks(Id));

IF OBJECT_ID(N'dbo.Sessions', N'U') IS NULL
CREATE TABLE dbo.Sessions (Id INT NOT NULL PRIMARY KEY IDENTITY, StackName NVARCHAR(255) NOT NULL, StackSize INT NOT NULL,
    Stack_Id INT NOT NULL, NumComplete INT NOT NULL, NumCorrect INT NOT NULL, AvgTime FLOAT NOT NULL, Date NVARCHAR(255) NOT NULL,
    FOREIGN KEY(Stack_Id) REFERENCES dbo.Stacks(Id));
GO