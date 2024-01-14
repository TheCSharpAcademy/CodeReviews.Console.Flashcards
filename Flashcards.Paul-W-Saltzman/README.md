# Flashcards

# Requirements
 - [x] This is an application where the users will create Stacks of Flashcards.
 - [x] You'll need two different tables for stacks and flashcards. The tables should be linked by a foreign key.
 - [x] Stacks should have an unique name.
 - [x] Every flashcard needs to be part of a stack. If a stack is deleted, the same should happen with the flashcard.
 - [x] You should use DTOs to show the flashcards to the user without the Id of the stack it belongs to.
 - [x] When showing a stack to the user, the flashcard Ids should always start with 1 without gaps between them. If you have 10 cards and number 5 is deleted, the table should show Ids from 1 to 9.
 - [x] After creating the flashcards functionalities, create a "Study Session" area, where the users will study the stacks. All study sessions should be stored, with date and score.
 - [x] The study and stack tables should be linked. If a stack is deleted, it's study sessions should be deleted.
 - [x]The project should contain a call to the study table so the users can see all their study sessions. This table receives insert calls upon each study session, but there shouldn't be update and delete calls to it.

 # Challenge
- [x]If you want to expand on this project, here’s an idea. Try to create a report system where you can see the number of sessions per month per stack. And another one with the average score per month per stack. This is not an easy challenge if you’re just getting started with databases, but it will teach you all the power of SQL and the possibilities it gives you to ask interesting questions from your tables.

# Features
- [x] SQL Studio database connection.
- [x] Selectable menu press up down and enter to navigate.
- [x] Crud Database options
- [x] Seed data loaded at first start up
- [x] Manage Stacks View Create and Delete Stacks
- [x] Manage Cards View Create and Delete Cards
- [x] Study Sessions
- [x] Study Session Reporting to view the past study sessions.

# What I learned
I spent quite a bit of time working through the connection to SSMS.  I also spent some time on pivot tables - this was new to me.  The menu took a bit of time.  The dynamic stack menu took more.   The expanding menu in cards took even longer.  I almost gave up on the last one, but I'm stuborn.

# References
https://www.youtube.com/watch?v=YyD1MRJY0qI
https://www.connectionstrings.com/sql-server-2019/
https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection.connectionstring?view=dotnet-plat-ext-7.0
https://erikej.github.io/dotnet/sqlclient/2022/11/17/sqlclient-dateonly.html
https://stackoverflow.com/questions/2762302/appropriate-datatype-for-holding-percent-values#:~:text=If%20you%20are%20going%20to%20store%20their%20face%20value%20(e.g.,with%20an%20appropriate%20CHECK%20constraint.
