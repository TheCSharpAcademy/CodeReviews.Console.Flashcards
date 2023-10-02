SET IDENTITY_INSERT dbo.Stacks on;
INSERT INTO Stacks (Id, Name) VALUES 
(1, 'French'),
(2, 'Spanish'),
(3, 'German');
SET IDENTITY_INSERT Stacks off;

INSERT INTO Flashcards (StackId, Front, Back) VALUES 
(1, 'À plus tard', 'See you later'),
(1, 'Au revoir', 'Bye'),
(1, 'Salut', 'Hi');

INSERT INTO Flashcards (StackId, Front, Back) VALUES 
(2, 'Hasta la vista', 'See you later'),
(2, 'Adios', 'Bye'),
(2, 'Hola', 'Hi');

INSERT INTO Flashcards (StackId, Front, Back) VALUES 
(3, 'Bis später', 'See you later'),
(3, 'Tschüß', 'Bye'),
(3, 'Hallo', 'Hi');