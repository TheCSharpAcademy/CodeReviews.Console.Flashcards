# Flashcards

## Table of Contents
- [General Info](#general-info)
- [Technologies](#technologies)
- [Features](#features)
- [Examples](#examples)
- [Instalation and Setup](#instalation-and-setup)
- [Requirements](#requirements)
- [Challenges](#challenges)
- [Things Learned](#things-learned)
- [Used Resources](#used-resources)

## General Info
Project made for @TheCSharpAcademy.  
This project is a basic flashcards application written in C# using SQL Server and Dapper for data access.  
It allows users to create stacks, add flashcards with questions and answers, see study sessions statistics, and practice their knowledge.

## Technologies
- C#
- SQL Server
- Dapper
- [Spectre.Console](https://github.com/spectreconsole/spectre.console)

## Features
- Create and manage stacks of flashcards.
- Add flashcards with questions and answers.
- Practise flashcards.
- Review study sessions statistics.
- User-friendly interface: Provides clear menus and prompts for interaction.
- Input validation: Ensure data entered by the user is valid.

## Examples
- Main Menu
- Manage Stacks
- Manage Flashcards
- Study Session
- View Study Sessions
- View monthly study session reports per stack:
	- Average Score
	- Number of sessions

## Instalation and Setup
1. Clone or download this project repository.
2. Open the solution file (Flashcards.Dejmenek.sln) in Visual Studio.
3. Install the required NuGet packages:
	- Dapper
	- System.Data.SqlClient
	- Spectre.Console
	- Spectre.Console.Cli
	- System.Configuration.ConfigurationManager
4. Update the App.config file with your SQL Server connection string details.
  
## Requirements
- [x] This is an application where the users will create Stacks of Flashcards.
- [x] You'll need two different tables for stacks and flashcards. The tables should be linked by a foreign key.
- [x] Stacks should have an unique name.
- [x] Every flashcard needs to be part of a stack. If a stack is deleted, the same should happen with the flashcard.
- [x] You should use DTOs to show the flashcards to the user without the Id of the stack it belongs to.
- [x] When showing a stack to the user, the flashcard Ids should always start with 1 without gaps between them. If you have 10 cards and number 5 is deleted, the table should show Ids from 1 to 9.
- [x] After creating the flashcards functionalities, create a "Study Session" area, where the users will study the stacks. All study sessions should be stored, with date and score.
- [x] The study and stack tables should be linked. If a stack is deleted, it's study sessions should be deleted.
- [x] The project should contain a call to the study table so the users can see all their study sessions. This table receives insert calls upon each study session, but there shouldn't be update and delete calls to it.

## Challenges
- [x] Try to create a report system where you can see the number of sessions per month per stack.
- [x] Try to create a report system where you can see the average score per month per stack.

## Things Learned
I have some previous experience with other DBMS like MySQL and SQLite so using SQL Server wasn't that hard.  
I only had to look up for data types and how to create pivot tables. Unfortunately, I encountered an issue with auto inrecementing ids in tables.  
When inserting a new record, the id would jump to 1000. It was weird, because it only occurred in the flashcards table.  
I removed all the tables and then recreated them. Now it works as intended😅.

I've also learned about Data Transfer Objects (DTOs).  
They provide a clean way to transfer data between different layers of an application, reducing the amount of unnecessary data sent.
In the future, I'd like to develop this project into a full-stack app with more features😁.

## Used Resources
- [SQL Server Tutorial](https://www.sqlservertutorial.net) used to learn some SQL Server syntax
- [C# Corner](https://www.c-sharpcorner.com/article/data-transfer-objects-dtos-in-c-sharp/) used to learn about DTOs