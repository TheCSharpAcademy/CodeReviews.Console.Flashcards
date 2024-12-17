# Flashcards.Bina28

This project is designed to manage flashcards, card stacks, and study-related sessions. It is built using C# and .NET, with SQL Server as the database. The database will be automatically filled with sample data upon initialization of the application.

## Features

- **Manage Flashcards**:  
  Users can choose a stack and perform the following actions:
  - View a specified number of flashcards. If the stack contains fewer flashcards than requested, a message will display.
  - Display all flashcards in a stack, with an appropriate message if no flashcards exist.
  - Create, edit, and delete flashcards.

- **Manage Card Stacks**:  
  Users can:
  - Create a new card stack.
  - Edit the name of an existing stack.
  - Delete a stack.

- **Manage Study Sessions**:  
  In this section, users can study:
  - Choose a stack.
  - Input the number of flashcards they want to study.
  - Translate words (from the chosen language) into English.
  - Receive feedback on whether the translation is correct.
  - After completing the session, the total score is displayed.

- **View Study Sessions**:  
  Users can view data about all completed study sessions. However, the data cannot be edited.

> **Note**: To see updates in tables, the application needs to be restarted.

## Requirements

- **SQL Server**: Ensure SQL Server is installed and running on your machine.
- **.NET SDK**: A compatible version of the .NET SDK is required.
- **IDE/Editor**: Visual Studio or another IDE that supports .NET development.

## Getting Started

1. **Clone the repository**:
   ```bash
   git clone <repository-url>
Open the project in your IDE.

Configure the database connection:

Edit the database settings in DBmanager/DBconfig.cs to match your SQL Server setup (e.g., username, password, database name).
Set up the database:

Run the provided SQL scripts to create the necessary tables and populate them with initial data.
Build and run the project:

Use your IDE to build and execute the application.
Project Structure
Controllers: Contains logic for managing flashcards, stacks, and study sessions.
DBmanager: Handles database configuration and interaction with SQL Server.
Models: Defines data models and Data Transfer Objects (DTOs) used in the application.
Helper.cs: Utility methods used throughout the application.
Program.cs: The entry point of the application.
UserInterface.cs: Manages user interactions and displays data.
Notes
Ensure SQL Server is running before starting the application.
Use the app.config file to configure any additional settings.