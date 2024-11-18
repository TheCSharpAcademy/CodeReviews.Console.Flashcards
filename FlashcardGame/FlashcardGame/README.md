## Flashcard project

Console based CRUD application to create stacks of flashcards. Created using Dapper, SQL, C#

## Given Requirements:

-This is an application where the users will create Stacks of Flashcards.

-You'll need two different tables for stacks and flashcards. The tables should be linked by a foreign key.

-Stacks should have an unique name.

-Every flashcard needs to be part of a stack. If a stack is deleted, the same should happen with the flashcard.

-You should use DTOs to show the flashcards to the user without the Id of the stack it belongs to.

-When showing a stack to the user, the flashcard Ids should always start with 1 without gaps between them. If you have 10 cards and number 5 is deleted, the table should show Ids from 1 to 9.

-After creating the flashcards functionalities, create a "Study Session" area, where the users will study the stacks. All study sessions should be stored, with date and score.

-The study and stack tables should be linked. If a stack is deleted, it's study sessions should be deleted.

-The project should contain a call to the study table so the users can see all their study sessions. This table receives insert calls upon each study session, but there shouldn't be update and delete calls to it.

 ## Features:

 -Program creates DB by itself and adds needed tables
 -You can add your stacks and when add flashcards to stack
 -You can go to study are to study your crated flashcards
 -You can check study sessions statistics by year

 ## Challenges

 -It was hard to understand how database works with my code, but i got used to it.
 -It was hard to organise my code correctly.
 -It was hard to pivot tables, but it worked i think well