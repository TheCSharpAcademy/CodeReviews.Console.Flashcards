
# Flashcards application
This is a flashcard application developed by using C# & SQL Server. The app consists of CRUD methods for Flashcards, their Stacks & their Study Sessions

## Requirements / Description
1) This is an application where the users will create Stacks of Flashcards
2) You'll need two different tables for stacks and flashcards. The tables should be linked by a foreign key
3) Stacks should have an unique name
4) Every flashcard needs to be part of a stack. If a stack is deleted, the same should happen with the flashcard
5) You should use DTOs to show the flashcards to the user without the Id of the stack it belongs to
6) When showing a stack to the user, the flashcard Ids should always start with 1 without gaps between them
7) After creating the flashcards functionalities, create a "Study Session" area, where the users will study the stacks. All study sessions should be stored, with date and score
8) The study and stack tables should be linked. If a stack is deleted, it's study sessions should be deleted
9) The project should contain a call to the study table so the users can see all their study sessions. This table receives insert calls upon each study session, but there shouldn't be update and delete calls to it

## Challenges
1) Create a report system where you can see the number of sessions per month per stack
2) Create a report with the average score per month per stack

## Before using the application
* After cloning the application, update the connection string in App.config to target your SQL Server
* In order to do this you should assign your server name to the *Data Source* property, and your DB name to the *Initial Catalog* property
* You will also have to create the SQL tables manually by opening MSSQL and running each of the queries found in the *Database* folder
 
## General Info
* The application consists of menus presenting CRUD options for Flashcards, Stacks & Study Sessions
* Each flashcard can belong to one stack, a stack can have many flashcards. Each study session can be studying on one stack, a stack can be studied by many study sessiosn.