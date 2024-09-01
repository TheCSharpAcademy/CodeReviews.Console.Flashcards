# Flashcards Console Application

## Description

This application is a command-line tool designed for creating, managing, and studying flashcards that are organized into stacks. It allows users to manage flashcards and stacks, conduct study sessions, and to track scores for previous study sessions.

## Features
<ul>
    <li>Stack Management: Create, update, delete, and view stacks of flashcards.</li>
    <li>Flashcard Management: Create, update, delete, and view flashcards within stacks.</li>
    <li>Study Sessions: Start study sessions by selecting a stack, answer questions, and receive feedback on answers.</li>
    <li>Score Tracking: View previous study sessions with their dates, stacks, and scores.</li>
</ul>

## Technologies
<ul>
    <li>C#:</li>
    <li>SQL Server: Database for storing stacks, flashcards, and study session data.</li>
    <li>Dapper: For database querying and interaction.</li>
    <li>Spectre.Console: For enhancing the console user interface with tables and panels.</li>
</ul>

## Setup and Installation

1. Clone the repository:

```bash
git clone https://github.com/gafoorraees/Flashcards.git 
cd Flashcards
```

2.  Set up the database:
 <ul>
	 <li>Make sure you have a running instance of SQL server.</li>
	 <li>Update the connection string in the application to point to your database.</li>
	 <li> Once the application is started, the database tables will be created if they don't exist.</li>
</ul>

3. Build and run the application.

