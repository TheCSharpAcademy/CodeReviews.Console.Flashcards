INSERT INTO Stacks (id, name) VALUES
(1, 'Science'),
(2, 'History'),
(3, 'Mathematics');

INSERT INTO Flashcards (id, stack_id, front, back) VALUES
(1, 1, 'What is photosynthesis?', 'The process by which green plants and some other organisms use sunlight to synthesize foods with the help of chlorophyll.'),
(2, 1, 'Name the planets in our solar system.', 'Mercury, Venus, Earth, Mars, Jupiter, Saturn, Uranus, Neptune'),
(3, 2, 'Who was the first President of the United States?', 'George Washington'),
(4, 2, 'When was the Declaration of Independence signed?', 'July 4, 1776'),
(5, 3, 'What is the value of pi (π)?', '3.14159');

INSERT INTO StudySessions (id, stack_id, study_time, score) VALUES
(1, 1, '2024-07-24 09:00:00', 85),
(2, 1, '2024-07-25 14:30:00', 92),
(3, 2, '2024-07-23 18:00:00', 88),
(4, 3, '2024-07-25 10:00:00', 95),
(5, 3, '2024-07-26 09:30:00', 90);
