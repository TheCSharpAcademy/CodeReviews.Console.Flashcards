# Flashcard App

A Console application developed with Visual Studio using C# and Microsoft SQL Server.

Users can create stacks of flashcards to aid with their studies! The program also allows users to track study sessions and check their answers for flashcard questions.


## Features

- Local SQL Server Database connection and manipulation using Dapper
    - Objects are created as DTOs for their respective table, then inserted into the database.
    - Entries read from the table will in turn create a DTO of that object type.

- Clean and Functional UI with Spectre Console
    - User can use arrow keys and enter to navigate most menus.
    - Flashcards, stacks, and study sessions are displayed nicely as tables on the console screen.

- CRUD Database functions
    - Users are able to create new flashcards and stacks of cards. They may also view, edit, and delete existing ones.
    - Foreign key constraints are accounted for by deleting all flashcards and study sessions tied to a stack when a stack is deleted.



## Tech Stack

**Runtime & Framework:** .NET 8

**Database:** Microsoft SQL Server

**ORM / Micro-ORM:** Dapper

**UI:** Spectre.Console


## Lessons Learned

- I went into this project wanting to try a different way of implementing a config file, so this app uses an appSettings.json file to store the connection string rather than the app.config approach. I found it slightly easier to implement this approach over App.config, even though both approaches are rather easy to use.

- Looking forward to soon learning the entity framework, I see what I believe is one trade off of using that over an ORM like Dapper. The approach I took had me hard coding a lot of SQL Queries. For a few tables, this is functional and a valid approach, but I can see this quickly spiraling out of control as the database scales up. Handling everything with C# through the entity framework appears to be one possible fix to this problem, but I can also appreciate that I have much finer control over the SQL that runs in this scenario.

- This was my first time interfacing with Microsoft SQL Server, setting up a local db with it, and getting Microsoft SQL Server Management Studio running. Coming from SQLite in previous projects and even earlier in my programming career PostgreSQL, it was a bit of an adjustment getting used to the syntax and how this flavor of SQL wanted me to handle specific operations. But aside from a few hiccups, one being figuring out how to check for an existing table before creating one, I felt at home fairly quick.

- I'm really happy with the small Utilities project I'm slowly building with this and one of my other projects, the coding tracker. I was able to reuse the dapper utility class I created and also add to it with a few functions I saw I was reusing a lot for this project in addition to noticing a few other utilities I could create to save time with the displayUtils and debugUtils classes. With further improvements, I could see this or something like it being used in a lot of my projects going forward.


## Acknowledgements

 - [The C# Academy](https://www.thecsharpacademy.com/)
 - [README Editor](https://readme.so/editor)
 - [Spectre Console](https://spectreconsole.net)
 - [Dapper](https://www.learndapper.com)
 - My cat, Pippin, for (attempting) to help by pressing random keys.

