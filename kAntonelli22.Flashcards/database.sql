USE FlashcardDB;

GO

-- DELETE FROM dbo.Stacks WHERE StackName = '{CurrentStack.StackName}'

-- GO

-- UPDATE dbo.Stacks 
--     SET StackName = 'Stack 5'
--     WHERE StackName = 'Stack 4'

-- GO

-- DROP TABLE dbo.Cards;
-- DROP TABLE dbo.Stacks;
-- DROP TABLE dbo.Sessions;

-- GO

-- CREATE TABLE dbo.Stacks (
--     Id INT NOT NULL PRIMARY KEY IDENTITY,
--     StackName NVARCHAR(255) NOT NULL,
--     StackSize INT NOT NULL
-- )

-- GO

-- CREATE TABLE dbo.Cards (
--     Id INT NOT NULL PRIMARY KEY IDENTITY,
--     Front NVARCHAR(255) NOT NULL,
--     Back NVARCHAR(255) NOT NULL,
--     Stack_Id INT NOT NULL,
--     FOREIGN KEY(Stack_Id) REFERENCES dbo.Stacks(Id)
-- )

-- GO

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