# Flashcards

- In this Project we created A Flashcards App. This App is developed
using MSSQL Server in [Docker](https://www.docker.com/) and
[Dapper](https://www.learndapper.com/) is
used as mini ORM.
- In this simple app users can create Stacks and
insert flashcards and study the Stacks. Users can also generate
some basic and advance reports. This Project is a part of
[CSharpAcademy](https://thecsharpacademy.com/) Console Projects.

## Requirements

- [x]This is an application where the users will create Stacks of Flashcards.
- [x]You'll need two different tables for stacks and flashcards. The tables
should be linked by a foreign key.
- [x]Stacks should have an unique name.
- [x]Every flashcard needs to be part of a stack. If a stack is deleted, the
same should happen with the flashcard.
- [x]You should use DTOs to show the flashcards to the user without the Id of
the stack it belongs to.
- [x]When showing a stack to the user, the flashcard Ids should always start
with 1 without gaps between them. If you have 10 cards and number 5 is deleted,
the table should show Ids from 1 to 9.
- [x]After creating the flashcards functionalities, create a "Study Session"
area, where the users will study the stacks. All study sessions should be
stored, with date and score.
- [x]The study and stack tables should be linked. If a stack is deleted,
it's study sessions should be deleted.
- [x]The project should contain a call to the study table so the users
can see all their study sessions. This table receives insert calls upon
each session, but there shouldn't be update and delete calls to it.

## Challenges

- [x] If you want to expand on this project, here’s an idea. Try to create a
report system where you can see the number of sessions per month per stack.
And another one with the average score per month per stack. This is not an
easy challenge if you’re just getting started with databases, but it will
teach you all the power of SQL and the possibilities it gives you to ask
interesting questions from your tables.

## App Usage

- [x] The use of the application is very straight forward. User is presented with
a Main Menu (Stack, Flashcard, Study, Reports, Exit) to select an Action.
After that a submenu is display to select and Action e.g. Add Stack, Edit
Stack or Delete Stack etc.
- [x] When user provide input to create, edit or generate a report
it is checked before performing the Action.
- [x] Users can't entered Duplicate Stacks and a Flashcard Front side
within the same Stack.

## Features

- [Spectre Console](https://spectreconsole.net/) library is used
to used to build a Menus and Submenus to navigate the application and to
visualize the reports in tables.

## Tools Used

- [x] [Docker](https://www.docker.com/) because I don't have
windows PC and ease of use
- [x] [Azure Data Studio](https://azure.microsoft.com/en-us/products/data-studio/)
- [x] [Spectre Console](https://spectreconsole.net/) can be used to create very
beautiful console apps.
