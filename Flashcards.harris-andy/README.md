# FlashCards_cSharpAcademy

This is my submission for the cSharpAcademy Flash Cards project found here: [Coding Tracker Project](https://thecsharpacademy.com/project/14/flashcards)


## Project Description
- A small console CRUD app in which the user can create flash cards, add them to a stack, play through study sessions and see basic study session stats. Data is stored through SQL Server.
- Built with C#/.Net 8, SQL Server, Dapper, Spectre Console, Azure Data Studio, Docker (thanks Microsoft for making SQL Server available on a Mac...)


## Usage
- Follow the instructions and away you go
- i.e. Select from the menu to perform operations such as: adding/deleting flash cards or stacks, studying flash cards and viewing study session stats.

![main menu](/Images/mainmenu3.png)

## Features
- Study flash cards. User's score is recorded in the database.
- Data is pulled through DTOs (where needed).
- Options to add fake flash cards, stacks and study sessions (ChatGPT helped with this SQL, it's a bit out of my pay grade).


![flash card](/Images/flashcard.png)


- Get summary records by year


![summary year](/Images/fcsummary.png)


- View study session history by subject


![study session history](/Images/studysessions.png)


## More to do
- UI formatting could use some work.
- The year summary function in the DisplayData class is clunky but I couldn't figure out how to make it more dynamic and shorter.


## New Stuff & Things I Learned. Neat!
- First time using SQL Server, Azure Data Studio and Docker (that was fun to set up...)
- Added a Controller class to improve design (finally got away from using "static" on everything)
- Using DTOs
- Whatever version of switch statements these are called. Much better than massive If statements!


## Questions & Comments
- I used AI for some of the table styling. I can't stand UI design so I gave up and let Claude make it pretty.
- I tried to improve my organization but would appreciate any suggestions for improvement.
- Should I add "bin/Debug/net8.0" and ".vscode" to the gitignore?
