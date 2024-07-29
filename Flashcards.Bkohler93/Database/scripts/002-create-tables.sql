SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

CREATE TABLE Stacks(
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Name VARCHAR(255) UNIQUE
)
WITH (DATA_COMPRESSION = ROW);

CREATE TABLE Flashcards (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StackId INT NOT NULL,
    Front VARCHAR(255),
    Back VARCHAR(255),
    FOREIGN KEY (StackId) REFERENCES Stacks(Id) ON DELETE CASCADE
)
WITH (DATA_COMPRESSION = ROW);

CREATE TABLE StudySessions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StackId INT NOT NULL,
    StudyTime DATETIME,
    Score INT,
    FOREIGN KEY (StackId) REFERENCES Stacks(Id) ON DELETE CASCADE
)
WITH (DATA_COMPRESSION = ROW);