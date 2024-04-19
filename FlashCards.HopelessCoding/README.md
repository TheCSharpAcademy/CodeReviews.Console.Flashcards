# Flashcards Console App

This is a one of the C# Academy projects. This application is used to create, manage and study stacks of flashcards.  
Users can create, edit and delete stacks and flashcards. In the study are users can study different stacks and
view records and data about their previous study sessions.

Below are the requirements, features, user manual, areas for improvement,  
and additional challenges of the application.

## Requirements

- [x] User capable to create stacks of flashcards.
- [x] Two different tables for stacks and flashcards which should be linked by a foreign key.
- [x] Stacks have unique name.
- [x] Every flashcard needs to be part of a stack. If a stack is deleted, the same should happen with the flashcard.
- [x] Use of DTOs to show the flashcards to the user without the Id of the stack it belongs to.
- [x] The flashcard Ids should always start with 1 without gaps between them whren shown for the user.
- [x] "Study sessions" are where the users can study the stacks. These should be stored with date and score.
- [x] The study and stack tables should be linked. If a stack is deleted, it's study sessions should be deleted.
- [x] The project should contain a call to the study table so the users can see all their study sessions.
      This table receives insert calls upon each study session, but there shouldn't be update
      and delete calls to it.

## Features and validations

- **SQL Server database connection**: The application connects to a SQL Server database
  to store and read data.
  - App.config file contains the configuration for that.
- **New Database Creation**: If the database or table doesn't exist, the application
  creates them when it starts.
- **Stacks and Flashcards**: Only limitation for Stack Name and Flashcards
  Front and Back is 50 character lenght. This could be easily changed if needed.
- **Spectre.Console -library utilization**: Used for menu selections, displaying
  record data to users, and enhancing console presentation with colors.

## User Manual

### Main Menu

- Users navigate the Main Menu by selecting options using arrow keys and
  confirming their selection by pressing Enter
![MainMenu](https://github.com/HopelessCoding/learning/assets/161690352/e539a710-ef54-4daa-acda-9122c91f7774)

- **Manage Stacks**: User can manage Stacks
- **Manage Flashcards**: User can manage Flashcards in Stack
- **Study**: Area where user can study Stacks and view records of the previous sessions
- **Exit**: Terminates the application

### Stacks Menu

- Users navigate the Stacks Menu by selecting options using arrow keys and
  confirming their selection by pressing Enter  
![StacksMenu](https://github.com/HopelessCoding/learning/assets/161690352/332da256-1f64-47a5-8521-818dbbcdfca2)
- When managing existing Stack user must choose the Stack by entering its name
![StackSelection](https://github.com/HopelessCoding/learning/assets/161690352/6960d850-271e-4885-af39-f62bc59badc1)
- If the Stack is deleted it will delete also all the Flashcards and Study Sessions of that Stack too
- If the Stack name is updated this will also update Stack name for Flashcards and Study Sessions of that Stack too  

### Flashcards Menu

- Users navigate the Flashcards Menu by selecting options using arrow keys and
  confirming their selection by pressing Enter
- User can manage Flashcards and view all or X number of Flashcards in selected Stack
![FlashcardsMenu](https://github.com/HopelessCoding/learning/assets/161690352/93e59d5b-e7eb-494f-980c-78f86874219d)

### Study Menu

- Users navigate the Study Menu by selecting options using arrow keys and
  confirming their selection by pressing Enter  
![StudyMenu](https://github.com/HopelessCoding/learning/assets/161690352/20482be2-5c5c-46ef-aaa4-eef7b3c44e3c)
- User can view different reports of the previous Study sessions
![Report](https://github.com/HopelessCoding/learning/assets/161690352/c60e712d-da44-4459-a869-a20129419c21)

## Areas for Improvement and Lessons Learned

- **Enhanced Code Quality and Structure**: This code is again a nice step forward for cleaner and better code.
  Still there is multiple things which I would like to do differently if doing this project
  again.
- **Spectre.Console -library**: Have been utilizing this now in three C# Academy projects
  and basic use starts to be easy. Still many things which I have never used in that library.
- **SQL and SQL Server**: Learned to connect and use SQL Server in C# projects. Learned lot of new things about
  SQL such as PIVOT. SQL is very powerful and nedd and want to learn it much more.
- **SQL Server Management Studio (SSMS)**: SMSS was already somewhat familiar for me but learned to
  use it much more. Especially use of queries for database management got much more familiar.

## Additional challenges

- [x] Report system where user can see the number of sessions per month per stack.
- [x] Report system where user can see the average score per month per stack.
