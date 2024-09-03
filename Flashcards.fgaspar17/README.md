# Flashcards

A console-based application to help you study via flashcards.
Developed using C#, Dapper, Spectre.Console and SQL Server Express LocalDB.

## Given Requirements

- [x] This is an application where the users will create Stacks of Flashcards.
- [x] You'll need two different tables for stacks and flashcards.
The tables should be linked by a foreign key.
- [x] Stacks should have an unique name.
- [x] Every flashcard needs to be part of a stack.
If a stack is deleted, the same should happen with the flashcard.
- [x] You should use DTOs to show the flashcards
to the user without the Id of the stack it belongs to.
- [x] When showing a stack to the user, the flashcard Ids should always
start with 1 without gaps between them.
If you have 10 cards and number 5 is deleted,
the table should show Ids from 1 to 9.
- [x] After creating the flashcards functionalities,
create a "Study Session" area, where the users will study the stacks.
All study sessions should be stored, with date and score.
- [x] The study and stack tables should be linked.
If a stack is deleted, it's study sessions should be deleted.
- [x] The project should contain a call to the study table
so the users can see all their study sessions.
This table receives insert calls upon each study session,
but there shouldn't be update and delete calls to it.

## Features

- SQL Server database connection
  
  - The data is stored in a SQL Server database, I connect for the CRUD.
  - If the database doesn't exist, it creates one.

- Console-based UI to navigate the menus
  
  - ![image](https://github.com/user-attachments/assets/0179dd0d-426b-45d9-a921-dc61d2a696c3)
  - ![image](https://github.com/user-attachments/assets/e812c5e1-5f65-471b-a4d7-691417ca2359)
  - ![image](https://github.com/user-attachments/assets/990ac3f3-e416-41f4-9ad9-b5db5adea9b5)

- CRUD operations
  
  - From the stack menu, you can create, show or delete stacks.
  - From the flashcard menu, you can create, update, show or delete flashcards.
  To choose an option you make use of arrow keys and enter.
  - Inputs are validated.
  - You can cancel an operation by entering the string from the configuration file.
  - ![image](https://github.com/user-attachments/assets/9360cc2b-66ff-4aa0-87bc-626cf4be4738)

- Study
  
  - After selecting a stack, the questions are presented,
  and you get feedback after answering.
  - ![image](https://github.com/user-attachments/assets/b4dca651-007c-417d-82f6-e7f854dc7103)
  - At the end, the score is presented.
  - ![image](https://github.com/user-attachments/assets/302af189-6d7e-4f65-b1e1-bfe0b4347377)

- Study Sessions

  - Every time you study a stack, a session is created, you need to choose a session,
  then the questions for that session are presented.
  - ![image](https://github.com/user-attachments/assets/4bf1c5f1-f988-4d5e-afa6-eebc6a1eb382)
  - ![image](https://github.com/user-attachments/assets/0bbd36d6-27a5-4072-a0c9-125fbad1087f)

- Monthly Report

  - You can choose between the number of sessions
  or the average score per month in a year.
  - ![image](https://github.com/user-attachments/assets/1018c02b-c8de-4794-80e7-90f0cf3fbce2)
  - ![image](https://github.com/user-attachments/assets/c41f124b-b16f-4d19-81b4-02b780ef8b9b)
  - ![image](https://github.com/user-attachments/assets/6d71e93e-8cc4-412d-bee8-248a2f0592d6)
 
## Challenges

- Creating the SQL Server Express LocalDB instance.
- Designing the database.
- Adding a way to cancel input for the user.
- Using DTOs to present the data.
- PIVOT data for monthly reports.

## Lessons Learned

- SQL Server Express LocalDB operations via mssqllocaldb.
- ON DELETE CASCADE keeps consistency in the database.
- DTOs present the data to the view in a user-friendly way.
- Microsoft.Extensions.Configuration.Binder is a library
to get values strongly typed from the configuration file.
- TypeConverter can be used to convert generic types.
- PIVOT operator in SQL is a great tool to analyze data.
- Attribute restrictions establish what can use the attribute.

## Areas to Improve

- Separating business logic to the services classes;
I feel like the view is doing business logic when it shouldn't.
- Database design to build scalable applications.
- SQL Server queries and administration to improve skills with a popular database.

## Resources used

- StackOverflow posts
- ChatGPT
- [PIVOT Documentation](https://learn.microsoft.com/en-us/sql/t-sql/queries/from-using-pivot-and-unpivot?view=sql-server-ver16)
- [DTO C#Corner Example](https://www.c-sharpcorner.com/article/data-transfer-objects-dtos-in-c-sharp/#:~:text=Data%20Transfer%20Objects%20(DTOs)%20play%20a%20pivotal%20role)
- [LocalDB Video](https://www.youtube.com/watch?v=M5DhHYQlnq8)
- [SQL Server Database Example](https://www.sqlservertutorial.net/getting-started/sql-server-sample-database/#:~:text=This%20tutorial%20provides%20you%20with%20a%20SQL%20Server)
