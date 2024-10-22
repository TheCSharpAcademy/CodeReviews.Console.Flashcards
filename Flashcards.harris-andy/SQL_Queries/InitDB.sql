IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('stacks') AND type in ('U'))
BEGIN
    CREATE TABLE stacks(
        Id INT PRIMARY KEY IDENTITY(1,1),
        name NVARCHAR(255) NOT NULL
    );
END;

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('flashcards') AND type in ('U'))
BEGIN
    CREATE TABLE flashcards (
        Id INT PRIMARY KEY IDENTITY(1,1),
        front NVARCHAR(255),
        back NVARCHAR(255),
        StackId INT,
        FOREIGN KEY (StackId) REFERENCES stacks(Id) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('study_sessions') AND type in ('U'))
BEGIN
    CREATE TABLE study_sessions (
        Id INT PRIMARY KEY IDENTITY(1,1),
        date DATETIME,
        score INT,
        questions INT,
        stackId INT,
        FOREIGN KEY (StackId) REFERENCES stacks(Id) ON DELETE CASCADE
    );
END;