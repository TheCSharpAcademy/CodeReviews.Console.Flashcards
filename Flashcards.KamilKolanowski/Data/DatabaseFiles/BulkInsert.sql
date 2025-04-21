USE Flashcards;
GO

-- Insert stacks
INSERT INTO Flashcards.TCSA.Stacks (StackName, Description)
VALUES
    ('C# Basics', 'Flashcards for basic C# syntax and concepts.'),
    ('SQL Essentials', 'Key SQL queries and functions.'),
    ('Data Structures', 'Core data structures and operations.');

-- Insert cards
INSERT INTO Flashcards.TCSA.Cards (StackId, FlashcardTitle, FlashcardContent)
VALUES
    (1, 'What is a class?', 'A class is a blueprint for objects.'),
    (1, 'What is an interface?', 'An interface defines a contract that classes can implement.'),
    (1, 'What is a constructor?', 'A special method used to initialize objects.'),
    (2, 'What does SELECT do?', 'It retrieves data from one or more tables.'),
    (2, 'What is a JOIN?', 'Combines rows from two or more tables based on related columns.'),
    (2, 'What does GROUP BY do?', 'Groups rows sharing a property for aggregate functions.'),
    (3, 'What is a stack?', 'LIFO data structure.'),
    (3, 'What is a queue?', 'FIFO data structure.'),
    (3, 'What is a hash table?', 'A data structure that maps keys to values for fast lookup.');

-- Insert study sessions
INSERT INTO Flashcards.TCSA.StudySessions (StackId, StackName, StartTime, EndTime, Score)
VALUES
    (1, 'C# Basics', '2025-04-22 08:00:00.0000000', '2025-04-22 08:15:00.0000000', 67),
    (2, 'SQL Essentials', '2025-04-21 14:00:00.0000000', '2025-04-21 14:25:00.0000000', 83),
    (2, 'SQL Essentials', '2023-04-21 14:00:00.0000000', '2023-04-21 14:25:00.0000000', 100),
    (3, 'Data Structures', '2025-04-20 18:30:00.0000000', '2025-04-20 18:45:00.0000000', 12);
