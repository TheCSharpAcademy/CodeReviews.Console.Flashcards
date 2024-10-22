-- Thanks ChatGPT!
IF NOT EXISTS (SELECT 1 FROM stacks WHERE name = 'Geography')
BEGIN
    INSERT INTO stacks (name) VALUES ('Geography');
END

IF NOT EXISTS (SELECT 1 FROM stacks WHERE name = 'Movie Trivia')
BEGIN
    INSERT INTO stacks (name) VALUES ('Movie Trivia');
END

IF NOT EXISTS (SELECT 1 FROM stacks WHERE name = 'SQL')
BEGIN
    INSERT INTO stacks (name) VALUES ('SQL');
END

DECLARE @GeographyStackId INT;
DECLARE @MovieTriviaStackId INT;
DECLARE @SQLStackId INT;

SELECT @GeographyStackId = Id FROM stacks WHERE name = 'Geography';
SELECT @MovieTriviaStackId = Id FROM stacks WHERE name = 'Movie Trivia';
SELECT @SQLStackId = Id FROM stacks WHERE name = 'SQL';

IF NOT EXISTS (SELECT 1 FROM flashcards WHERE StackId = @GeographyStackId)
BEGIN
    INSERT INTO flashcards (front, back, StackId)
    VALUES 
    ('What is the capital of France?', 'Paris', @GeographyStackId),
    ('What is the largest continent?', 'Asia', @GeographyStackId),
    ('What is the longest river?', 'Nile', @GeographyStackId),
    ('What country has the most population?', 'China', @GeographyStackId),
    ('What is the smallest country?', 'Vatican City', @GeographyStackId),
    ('What is the largest desert?', 'Sahara', @GeographyStackId),
    ('Which ocean is the deepest?', 'Pacific Ocean', @GeographyStackId),
    ('Which mountain is the highest?', 'Mount Everest', @GeographyStackId),
    ('What is the coldest continent?', 'Antarctica', @GeographyStackId),
    ('What is the capital of Japan?', 'Tokyo', @GeographyStackId);
END

IF NOT EXISTS (SELECT 1 FROM flashcards WHERE StackId = @MovieTriviaStackId)
BEGIN
    INSERT INTO flashcards (front, back, StackId)
    VALUES 
    ('Who directed the movie "Inception"?', 'Christopher Nolan', @MovieTriviaStackId),
    ('What year was the original "Jurassic Park" released?', '1993', @MovieTriviaStackId),
    ('Who played the character of Jack in "Titanic"?', 'Leonardo DiCaprio', @MovieTriviaStackId),
    ('Which movie won the Oscar for Best Picture in 2020?', 'Parasite', @MovieTriviaStackId),
    ('Who played the Joker in "The Dark Knight"?', 'Heath Ledger', @MovieTriviaStackId),
    ('In which movie did Tom Hanks play a character stranded on an island?', 'Cast Away', @MovieTriviaStackId),
    ('What is the highest-grossing movie of all time?', 'Avatar', @MovieTriviaStackId),
    ('Which animated movie features a talking snowman named Olaf?', 'Frozen', @MovieTriviaStackId),
    ('Who directed the "Lord of the Rings" trilogy?', 'Peter Jackson', @MovieTriviaStackId),
    ('What is the name of the spaceship in "Alien"?', 'Nostromo', @MovieTriviaStackId);
END

IF NOT EXISTS (SELECT 1 FROM flashcards WHERE StackId = @SQLStackId)
BEGIN
    INSERT INTO flashcards (front, back, StackId)
    VALUES 
    ('What does SQL stand for?', 'Structured Query Language', @SQLStackId),
    ('What is the purpose of a SELECT statement?', 'To retrieve data from a database', @SQLStackId),
    ('What is the function of WHERE clause?', 'Filters records based on a condition', @SQLStackId),
    ('What does JOIN do in SQL?', 'Combines rows from two or more tables', @SQLStackId),
    ('What is the purpose of GROUP BY?', 'Groups rows that share a property', @SQLStackId),
    ('What is a primary key?', 'A unique identifier for a record', @SQLStackId),
    ('What is a foreign key?', 'A field that links to the primary key of another table', @SQLStackId),
    ('What does INSERT INTO do?', 'Adds new records to a table', @SQLStackId),
    ('How do you update data in SQL?', 'Using the UPDATE statement', @SQLStackId),
    ('What does DELETE FROM do?', 'Removes records from a table', @SQLStackId);
END
