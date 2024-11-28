# Flashcards Project

## PROJECT OVERVIEW
This is a console-based CRUD application for creating, managing, and studying flashcards, built with **C#**, **Dapper**, **MSSQL**, and **Spectre.Console**. The application allows users to create flashcards, organise them into stacks, and study using an intuitive console-based UI.

The application is designed to improve learning by providing features like:
- Flashcard creation and organisation.
- Interactive study sessions with progress tracking.
- Comprehensive reporting.

**Note**: 
To run this project locally, setup database with instructions below and ensure the database path in the configuration file is updated to point to your MSSQL instance. This ensures the application can connect to the correct database for all CRUD operations.

*Setting Up the Database*

**Step 1**: Create a Database
Open your MSSQL Server Management Studio (SSMS) or your preferred SQL client.

Run the following script to create a new database:
CREATE DATABASE FlashCardsDB;

**Step 2**: Update the Configuration File

Locate the configuration file in your project (App.Config).

Update the connection string to point to your MSSQL instance and the newly created database.

Example:


        <add name="DatabasePath" connectionString="Server={your-instance};Database=FlashCardsDb;Trusted_Connection=True;TrustServerCertificate=True;" providerName="Microsoft.Data.SqlClient" />



Replace the following placeholders in the connection string:

YOUR_SERVER_NAME with your local MSSQL server name or instance.

YOUR_USERNAME and YOUR_PASSWORD with valid credentials for your database.
---
## Requirements
- This is an application where the users will create Stacks of Flashcards.
- Two different tables for stacks and flashcards. The tables should be linked by a foreign key.
- Stacks should have an unique name.
- Every flashcard needs to be part of a stack. If a stack is deleted, the same should happen with the flashcard.
- Use DTOs to show the flashcards to the user without the Id of the stack it belongs to.
- When showing a stack to the user, the flashcard Ids should always start with 1 without gaps between them. If you have 10 cards and number 5 is deleted, the table should show Ids from 1 to 9.
- After creating the flashcards functionalities, create a "Study Session" area, where the users will study the stacks. All study sessions should be stored, with date and score.
- The study and stack tables should be linked. If a stack is deleted, it's study sessions should be deleted.
- The project should contain a call to the study table so the users can see all their study sessions. This table receives insert calls upon each study session, but there shouldn't be update and delete calls to it.


## Features
- The program connects to an MSSQL database to store and retrieve information.
- It uses Dapper Micro-ORM for executing SQL queries and mapping the results to Data Transfer Objects, ensuring efficient and clean data handling.
- If the database or the required tables are missing, they are automatically created when the program starts.
- Console-Based UI
- The application features a user-friendly console-based interface built with Spectre.Console.
- Users can navigate menus intuitively by selecting options with their keyboard, providing a seamless experience.


#  DB Functionality

###  Main Menu
The main menu allows users to navigate between the core functionalities:
- Manage Stacks
- Manage Flashcards
- Study
- View Study Sessions Data
- Manage Stacks
---------------------------------------------------

### Perform CRUD operations on stacks:
- Create: Add a new stack with a unique name.
- View: Display a list of all existing stacks.
- Update: Rename an existing stack.
- Delete: Remove a stack and its associated flashcards and study data.
- Note: All stack names must be unique.

---------------------------------------------------
### Manage Flashcards
Features for managing flashcards:
- View All Cards: Display all flashcards in the current stack.
- View Select Amount: Display a specified number of flashcards.
- Change Current Stack: Switch to a different stack for managing or studying.
- Create: Add new flashcards to the current stack.
- Edit: Modify the front or back of an existing flashcard.
- Delete: Remove a specific flashcard from the stack.

---
### Study
Engage in an interactive study session:
- Choose a Stack: Select a stack of flashcards for the session.
- Interactive Study Loop: A random flashcard is displayed using Spectre.Console, and the user is prompted to guess the answer.
- Exit Option: Users can press 0 at any time to exit the study session.
- Score Summary: Upon exiting, the user’s score is displayed, and the session data is automatically saved to the database.

---------------------------------------------------
### View Study Sessions Data
Analyze study performance with detailed reports:
1. Input Stack Name: Select the stack for which you want to view reports.
2. Choose Report Type:
- Number of Sessions Per Month: View how many study sessions occurred each month.
- Average Score Per Month: See the average scores achieved each month.
3. Input Year: Provide the year for which you want the report.
4. Display Results: Reports are shown using Spectre.Console for a clean, tabular format.
