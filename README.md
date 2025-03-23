# Flashcards

Welcome to the Flashcards App!

![icons8-quizlet-96](https://github.com/user-attachments/assets/022576de-826a-4f4a-82eb-54923adcd237)


This is a .NET project designed to demonstrate using SQL Server to store and retain data, in the form of revision-style flashcards. The project also features the use of DTO'S and Pivoting a Table

Flashcards is a console app using .NET and SQL
The user can add their own revision-style flashcards to help revise a variety of subjects, this is followed up with a study session and a history of the user's scores!

The front end is a console app.
There is an integrated SQL Server database in the back end.

## Requirements
This application fulfils the following The C# Academy - Flashcards App requirements:

1) This is an application where the users will create Stacks of Flashcards.
2) You'll need two different tables for stacks and flashcards. The tables should be linked by a foreign key.
3) Stacks should have a unique name.
4) Every flashcard needs to be part of a stack. If a stack is deleted, the same should happen with the flashcard.
5) You should use DTOs to show the flashcards to the user without the ID of the stack it belongs to.
6) When showing a stack to the user, the flashcard IDs should always start with 1 without gaps between them. If you have 10 cards and the number 5 is deleted, the table should show IDs from 1 to 9.
7) After creating the flashcards functionalities, create a "Study Session" area, where the users will study the stacks. All study sessions should be stored, with date and score.
8) The study and stack tables should be linked. If a stack is deleted, its study sessions should be deleted.
9) The project should contain a call to the study table so the users can see all their study sessions. This table receives insert calls upon each study session, but there shouldn't be updates or deleted calls to it.


## Challenges
This project has the following challenges:
1) Try to create a report system where you can see the number of sessions per month per stack.
2) A report showing the average score per month

## Technologies
* .NET
* C# (.Net 8.0)
* SQL Server 2022

## Nuget Packages:
* Dapper
* Spectre Console
* System Text Json
* Microsoft Data SQL Client
* Microsoft Extensions Configuration
* Microsoft Extensions Configuration Abstractions
* Microsoft Extensions Configuration Json
* Microsoft Sql Server Sever

## What I learned from this project
1) Appsettings configuration JSON files and where to store them
2) installing and setting up SQL Server 2022
3) Using SQL Server Management Suite
4) SQL Server tools and toolbars in Visual Studio
5) Key value pairs with a Dictionary file
6) DTO's
7) Pivoting Tables
8) More usage of SQL

## Getting Started
# IMPORTANT NOTE:

The initial Database creation SQL  has been added, but a database called Flashcards does need to be added to SQL server for the query to create the necessary tables

## Prerequisites
* .NET 8 SDK
* An IDE (code editor) like Visual Studio or Visual Studio Code.
* A database management tool (optional).

## Installation
Clone the repository:

git clone https://github.com/RyanW84/CodeReviews.Console.Flashcards.git

##Configure the application:

You can call the SeedData() method in Program.CS when running for the first time to add some example questions and answers. Once this has been done the first time, it can be commented out.

Update the connection string in appsettings.json to target your SQL Server (localDB)
Build the application using the .NET CLI:

dotnet build

##Running the Application
You can run the Web application from Visual Studio.
OR
Run using the .NET CLI in the folder you have chosen when cloning

## Usage
Please refer to the short demonstration below

![FlashcardsPreview](https://github.com/user-attachments/assets/b1f61780-42dd-44f9-b074-925a1d3c8796)


## How It Works
1) The console app connects to the database and confirms the connection is open
2) The menu system is presented to the user to make a choice.
3) Each menu option has a method that performs the function selected, in some cases such as adding an item there will be a User Interface Method to gather the information from the user, and back end method to act on the database
4) The data is stored in three tables: Stacks, Flashcards and Study Session, they are linked using StackID as the Foreign Key (See the Entity Relationship Diagram below)
5) The app stores and retrieves data from these tables.
6) During the study session, DTOs are used to conceal back-end data and only show what the user needs to see.
7) The study session calculates a percentage of how the user has done during the session, this is calculated and persistently stored by the SQL DB
8) The report section gathers Study Session Data to present it in a more information-friendly way, showing the user a monthly breakdown of how many study sessions they have done
9) The app is exited through the Main Menu

## Database
### entity-relationship-diagram

![SSMS ER DIAGRAM](https://github.com/user-attachments/assets/8b3d008c-cbec-4d4e-bd5c-db17afb52871)


## Solution Diagram showing all Methods and classes (generated using Mermaid Design)

![Flashcards Mermaid Diagram](https://github.com/user-attachments/assets/e8659910-751f-47c6-b525-4fd7e40040c3)



## Credits
Many thanks as always to Pablo De Souza from The C Sharp Academy for his mentoring and guidance
