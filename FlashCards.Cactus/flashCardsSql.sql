USE flashcardsdb;

-- Create a new table called 'Stack' in schema 'flashcardsdb.dbo'
-- Drop the table if it already exists
IF OBJECT_ID('flashcardsdb.dbo.Stack', 'U') IS NOT NULL
DROP TABLE flashcardsdb.dbo.Stack
GO
-- Create the table in the specified schema
CREATE TABLE flashcardsdb.dbo.Stack
(
    sid INT NOT NULL PRIMARY KEY IDENTITY(1, 1), -- primary key column
    name [NVARCHAR](50) UNIQUE NOT NULL,
);
GO

-- Create a new table called 'FlashCard' in schema 'flashcardsdb.dbo'
-- Drop the table if it already exists
IF OBJECT_ID('flashcardsdb.dbo.FlashCard', 'U') IS NOT NULL
DROP TABLE flashcardsdb.dbo.FlashCard
GO
-- Create the table in the specified schema
CREATE TABLE flashcardsdb.dbo.FlashCard
(
    fid INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    front VARCHAR(200),
    back VARCHAR(200),
    sid INT FOREIGN KEY REFERENCES Stack(sid)
);
GO

-- Create a new table called 'StudySession' in schema 'flashcardsdb.dbo'
-- Drop the table if it already exists
IF OBJECT_ID('flashcardsdb.dbo.StudySession', 'U') IS NOT NULL
DROP TABLE flashcardsdb.dbo.StudySession
GO
-- Create the table in the specified schema
CREATE TABLE flashcardsdb.dbo.StudySession
(
    ssid INT NOT NULL PRIMARY KEY IDENTITY(1, 1) , -- primary key column
    sid INT FOREIGN KEY REFERENCES Stack(sid),
    date DATETIME,
    score Int,
);
GO

SELECT @@VERSION AS 'SQL Server Version';