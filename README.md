# Flashcards
- Console based CRUD application for creating and studing flashcards
- Developed using C# and MSSQL

# Given Requirements:
- [x]  This is an application where the users will create Stacks of Flashcards.
- [x]  You'll need two different tables for stacks and flashcards. The tables should be linked by a foreign key.
- [x]  Stacks should have an unique name.
- [x]  Every flashcard needs to be part of a stack. If a stack is deleted, the same should happen with the flashcard.
- [x]  You should use DTOs to show the flashcards to the user without the Id of the stack it belongs to.
- [x]  When showing a stack to the user, the flashcard Ids should always start with 1 without gaps between them. If you have 10 cards and number 5 is deleted, the table should show Ids from 1 to 9.
- [x]  After creating the flashcards functionalities, create a "Study Session" area, where the users will study the stacks. All study sessions should be stored, with date and score.
- [x]  The study and stack tables should be linked. If a stack is deleted, it's study sessions should be deleted.
- [x]  The project should contain a call to the study table so the users can see all their study sessions. This table receives insert calls upon each study session, but there shouldn't be update and delete calls to it.

# Features
* Application connects to a MSSQL server
* If not present it will create a new database and tables
* Easy to navigate console ui using Spectre.Console which allows users to navigate the application with their keyboard and arrow keys
* ![image](https://github.com/user-attachments/assets/934f93b1-4edb-4486-a9a9-8e9aa2643bb1)
* View, create, modify, and delete stacks
* ![image](https://github.com/user-attachments/assets/d0125252-3e0f-485d-86d1-22abd91cdc2a)
* ![image](https://github.com/user-attachments/assets/440e4476-f562-4a6c-93e4-350d0e6371b5)
* Practice stacks and save them as study sessions
* ![image](https://github.com/user-attachments/assets/198a77fb-0509-4314-859d-1db7caebf651)
* View Past study sessions
* ![image](https://github.com/user-attachments/assets/f7a8abac-2037-454e-a0ba-6e3e5abc5a0d)

# Resources Used
* assorted stack overflow questions
* Spectre.Console
* Dapper ORM
