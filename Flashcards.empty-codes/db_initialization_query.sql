-- Create the database
CREATE DATABASE myFlashCardsDb;
GO

-- Use the database
USE myFlashCardsDb;
GO

-- Create the Stacks table
CREATE TABLE Stacks (
    StackId INT PRIMARY KEY IDENTITY(1,1),
    StackName VARCHAR(255) NOT NULL UNIQUE
);
GO

-- Create the Flashcards table
CREATE TABLE Flashcards (
    FlashcardId INT PRIMARY KEY IDENTITY(1,1),
    Question VARCHAR(255),
    Answer VARCHAR(255),
    StackId INT,
    FOREIGN KEY (StackId) REFERENCES Stacks(StackId) ON DELETE CASCADE
);
GO

-- Create the StudySessions table
CREATE TABLE StudySessions (
    SessionId INT PRIMARY KEY IDENTITY(1,1),
    StudyDate DATETIME DEFAULT GETDATE(),
    Score VARCHAR(255),
    StackId INT,
    FOREIGN KEY (StackId) REFERENCES Stacks(StackId) ON DELETE CASCADE
);
GO

-- Insert stacks
INSERT INTO Stacks (StackName) VALUES 
('Greetings in French'),
('Greetings in Spanish');
GO

-- Insert flashcards for French greetings
INSERT INTO Flashcards (Question, Answer, StackId) VALUES 
('Bonjour', 'Good morning', 1),
('Bonsoir', 'Good evening', 1),
('Salut', 'Hi', 1),
('Merci', 'Thank you', 1),
('Au revoir', 'Goodbye', 1);
GO

-- Insert flashcards for Spanish greetings
INSERT INTO Flashcards (Question, Answer, StackId) VALUES 
('Hola', 'Hello', 2),
('Buenos días', 'Good morning', 2),
('Buenas tardes', 'Good afternoon', 2),
('Buenas noches', 'Good evening', 2),
('Adiós', 'Goodbye', 2);
GO

-- Insert study sessions for French greetings
INSERT INTO StudySessions (StudyDate, Score, StackId) VALUES 
(GETDATE(), 'You got 4 correct out of 5 questions.', 1),
(GETDATE(), 'You got 3 correct out of 5 questions.', 1),
(GETDATE(), 'You got 5 correct out of 5 questions.', 1);
GO

-- Insert study sessions for Spanish greetings
INSERT INTO StudySessions (StudyDate, Score, StackId) VALUES 
(GETDATE(), 'You got 2 correct out of 5 questions.', 2),
(GETDATE(), 'You got 4 correct out of 5 questions.', 2),
(GETDATE(), 'You got 3 correct out of 5 questions.', 2);
GO
