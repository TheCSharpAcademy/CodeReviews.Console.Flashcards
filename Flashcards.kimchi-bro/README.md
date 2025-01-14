### Check if you have on your local machine installed:
- MS SQL Server Express
- SQL Server Management Studio (SSMS)
- SQL Server Express 2019 LocalDB as individual component in your VS
# Setup
- Create `Flashcards` instance of a local SQL server:
    - run command `sqllocaldb create "Flashcards"` in Windows Terminal
- Open SSMS and connect to `Flashcards` local server instance:
    - select `(localdb)\Flashcards` for server name
- Create new `FlascardDB` database in local server instance:
    - run SQL query `CREATE DATABASE FlashcardDB;`
- Build and run app
# Features
- Three different tables for stacks, flashcards and study sessions
- Tables are linked by a foreign key
- If a stack is deleted, all flashcards and study sessions linked to it are deleted too
- Using DTOs for showing stacks, flashcards and sessions

# Mock Area
- Flashcards generation
- Random study session generation