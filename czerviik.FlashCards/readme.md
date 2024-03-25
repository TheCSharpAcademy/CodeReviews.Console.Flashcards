# Console Flashcards project

## Introduction

Console based CRUD application where the users will create Stacks of Flashcards, make Study Sessions and check Reports.
Developed using C# and SQL Server.
* https://www.thecsharpacademy.com/project/14

## Given Requirements
<img width="564" alt="Screen Shot 2024-03-25 at 2 30 40 PM" src="https://github.com/czerviik/CodeReviews.Console.Flashcards/assets/137193704/2ba0ff7b-3cc6-4b76-9f67-7745af258358">



## Given Challenges

<img width="572" alt="Screen Shot 2024-03-25 at 2 31 16 PM" src="https://github.com/czerviik/CodeReviews.Console.Flashcards/assets/137193704/ce977822-9046-4111-a9d1-2da6e3592eb1">

## Features

### SQL Server

* The program uses a SQL Server db connection to store and read information.
* The program uses three tables: 'flashcards', 'stacks' and 'Study_sessions'

### A SpectreConsole based console based UI where users can navigate by arrow keys

![Screen Shot 2024-03-25 at 2 33 39 PM](https://github.com/czerviik/CodeReviews.Console.Flashcards/assets/137193704/ac549fca-28fe-49d0-870d-d0407d6e32cf)

### Displaying tables using the Spectre library

![Screen Shot 2024-03-25 at 2 34 37 PM](https://github.com/czerviik/CodeReviews.Console.Flashcards/assets/137193704/fa5f10f5-b67e-4964-9535-7d313c1ad310)
  

### Managing flashcards
#### Users can:

* add one or more flashcards
* choose existing flashcard stack, or automatically create a new one
* display all flashcards in specific time range or filtered by stacks
* update or delete existing flashcards

### Reports

* reports are displayed in tables

![Screen Shot 2024-03-25 at 2 56 47 PM](https://github.com/czerviik/CodeReviews.Console.Flashcards/assets/137193704/0e8e908d-6f6b-4486-89db-9f29aa4e131d)

* for grouping and displaying reports data, LINQ menthods have been used
instead of SQL Server queries (including pivoting tables)


## Lessons Learned

* I created a basic diagram scheme before starting the developement
  <img width="433" alt="Screen Shot 2024-03-25 at 2 53 27 PM" src="https://github.com/czerviik/CodeReviews.Console.Flashcards/assets/137193704/edcd9dec-43ac-48e9-b13d-a837f626f441">
* in case of UI, this time I relied completely on SpectreConsole Selections and Tables
* running SQL Server was quite a tough nut to crack as it's not supported by current MacOS.
  I had to install Docker, set up a Linux container and install SQL Server there.
* I also used Azure Data Studio instead of Microsoft SQL Server Management Studio for visualization the database.
* I had to learn about DTOs and their purposes, also used Dapper ORM for the first time
* Tried to get a grasp on Entinty Framework but gived up, keeping it for later study
* With the Challenge part of the project itself, It naturally led me to use LINQ instead of SQL Server Queries,
  so I studied quite a bit about using LINQ and lambda functions
  
