SET IDENTITY_INSERT stacks on;
INSERT INTO stacks (id, name) VALUES 
(1, 'French'),
(2, 'Spanish'),
(3, 'German');
SET IDENTITY_INSERT stacks off;

INSERT INTO flashcards (stack_id, front, back) VALUES 
(1, 'À plus tard', 'See you later'),
(1, 'Au revoir', 'Bye'),
(1, 'Salut', 'Hi');

INSERT INTO flashcards (stack_id, front, back) VALUES 
(2, 'Hasta la vista', 'See you later'),
(2, 'Adios', 'Bye'),
(2, 'Hola', 'Hi');

INSERT INTO flashcards (stack_id, front, back) VALUES 
(3, 'Bis später', 'See you later'),
(3, 'Tschüß', 'Bye'),
(3, 'Hallo', 'Hi');