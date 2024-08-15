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
  ![image](https://github.com/user-attachments/assets/934f93b1-4edb-4486-a9a9-8e9aa2643bb1)
* View, create, modify, and delete stacks
* Practice stacks and save them as study sessions
* View Past study sessions
  ![image](https://github.com/user-attachments/assets/c8f35e42-c492-4603-bd7b-1cf618d96919)


# Resources Used
* assorted stack overflow questions
* Spectre.Console
* Dapper ORM
