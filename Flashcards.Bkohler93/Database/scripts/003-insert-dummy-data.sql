SET NOCOUNT ON;

INSERT INTO dbo.Stacks (Name) VALUES
("Science"),
("History"),
("Mathematics");

INSERT INTO dbo.Flashcards (StackId, Front, Back) VALUES
(1, "What is photosynthesis?", "The process by which green plants and some other organisms use sunlight to synthesize foods with the help of chlorophyll."),
(1, "Name the planets in our solar system.", "Mercury, Venus, Earth, Mars, Jupiter, Saturn, Uranus, Neptune"),
(2, "Who was the first President of the United States?", "George Washington"),
(2, "When was the Declaration of Independence signed?", "July 4, 1776"),
(3, "What is the value of pi (π)?", "3.14159");

INSERT INTO dbo.StudySessions (StackId, StudyTime, Score) VALUES
(1, "2024-07-24 09:00:00", 85),
(1, "2024-07-25 14:30:00", 92),
(2, "2024-07-23 18:00:00", 88),
(3, "2024-07-25 10:00:00", 95),
(3, "2024-07-26 09:30:00", 90);
