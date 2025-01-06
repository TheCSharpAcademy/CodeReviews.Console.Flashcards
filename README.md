# FlashCards
- Console based CRUD application to create and study flashcards
- Developed using C# and MSSQL

# Given Requirements:
- [x] This is an application where the users will create Stacks of Flashcards.
- [] You'll need two different tables for stacks and flashcards. The tables should be linked by a foreign key.
- [] Stacks should have an unique name.
- [] Every flashcard needs to be part of a stack. If a stack is deleted, the same should happen with the flashcard.
- [] You should use DTOs to show the flashcards to the user without the Id of the stack it belongs to.
- [] When showing a stack to the user, the flashcard Ids should always start with 1 without gaps between them. If you have 10 cards and number 5 is deleted, the table should show Ids from 1 to 9.
- [] After creating the flashcards functionalities, create a "Study Session" area, where the users will study the stacks. All study sessions should be stored, with date and score.
- [] The study and stack tables should be linked. If a stack is deleted, it's study sessions should be deleted.
- [] The project should contain a call to the study table so the users can see all their study sessions. This table receives insert calls upon each study session, but there shouldn't be update and delete calls to it.

# Features

* MSSQL database connection
		
- The program uses a MSSQL db conneciton to store and read information.
- If no database exists, or the correct table does not exist, they will be created when the program starts.

* A console based UI using Spectre Console where users can navigate with selecting with their keyboard
