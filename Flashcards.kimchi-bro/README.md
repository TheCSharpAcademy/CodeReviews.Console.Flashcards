### Check if you have on your local machine installed:
- MS SQL Server Express
- SQL Server Management Studio (SSMS)
- SQL Server Express 2019 LocalDB as individual component in your VS
# Setup
- Run command `sqllocaldb create "Flashcards"` in Windows Terminal to create `Flashcards` instance of a local SQL server
- Connect to `Flashcards` local server instance in SSMS: select `(localdb)\Flashcards` for server name
- Run SQL query `CREATE DATABASE FlashcardDB;` to create new `FlascardDB` database in local server instance
- Build and run app
# Features
- Three different tables for stacks, flashcards and study sessions
- Tables are linked by a foreign key
- If a stack is deleted, all flashcards and study sessions linked to it are deleted too
- Using DTOs for showing stacks, flashcards and sessions
# Mock Area
- Flashcards generation
- Random study session generation