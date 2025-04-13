# Flashcards App
The **Flashcards App** is an interactive console application
designed to help you create, manage, and study various topics efficiently.

## Given Requirements

- This is an application where the users will create Stacks of
  Flashcards.
- We should use Spectre.Console to print tables and collect inputs.
- You'll need two different tables for stacks and flashcards.
- The tables should be linked by a foreign key.
- Stacks should have an unique name.
- Every flashcard needs to be part of a stack. If a stack is deleted,
  the same should happen with the flashcard.
- You should use DTOs to show the flashcards to the user without the
  Id of the stack it belongs to.
- When showing a stack to the user, the flashcard Ids should always
  start with 1 without gaps between them.
- If you have 10 cards and number 5 is deleted, the table should
  show Ids from 1 to 9.
- After creating the flashcards functionalities, create a "Study Session"
  area, where the users will study the stacks.
- All study sessions should be stored, with date and score.
- The study and stack tables should be linked. If a stack is deleted,
  it's study sessions should be deleted.
- The project should contain a call to the study table so the users
  can see all their study sessions.
- This table receives insert calls upon each study session, but there
  shouldn't be update and delete calls to it.

## Features

- MSSQL database connection
- The program uses a MSSQL db conneciton to store and read information.
- If no database exists, or the correct table does not exist, they will
  be created when the program starts.

## Console Based UI

- This program features a text based menu and navigation system to access
  its function
- It utilizes the Spectre.Console library to generate the main menu and
  display text and sessions in the program
  ![Screenshot of the main menu of the application.](https://rvnprojectstorage.blob.core.windows.net/images/Console.Flashcards/MainMenu.png)

## CRUD DB Functions

- This program offers CRUD operations (Create, Read, Update, Delete) for
  both Flashcards and Stacks
- You can create new flashcards with a question and answer
- Then each flashcard can be placed into a stack to be used in a study session
- Flashcards and stacks can be created, updated or removed as needed
- Here are screenshots from some of the various operations
  |View Flashcards|View Stacks|
  |:-:|:-:|
  |![View Flashcards](https://rvnprojectstorage.blob.core.windows.net/images/Console.Flashcards/FlashCardsMenu.png)|![View Stacks](https://rvnprojectstorage.blob.core.windows.net/images/Console.Flashcards/ViewStacks.png)|
  |Add Flashcard|Remove Flashcard|
  |![Add a Flashcard](https://rvnprojectstorage.blob.core.windows.net/images/Console.Flashcards/AddFlashcard.png)|![Remove a flash card](https://rvnprojectstorage.blob.core.windows.net/images/Console.Flashcards/DeleteFlashcard.png)|
  |Add Stack|Remove Stack|
  |![Add Stack](https://rvnprojectstorage.blob.core.windows.net/images/Console.Flashcards/AddStack.png)|![Remove Stack](https://rvnprojectstorage.blob.core.windows.net/images/Console.Flashcards/DeleteStack.png)|

## Study Session

- This program features the ability to run a study session.
- It will go through a stack, ask you each question and give a score at the end
  |Start Session|Completed Session|
  |:-:|:-:|
  |![Start Session](https://rvnprojectstorage.blob.core.windows.net/images/Console.Flashcards/StartStudySession.png)|![Completed Session](https://rvnprojectstorage.blob.core.windows.net/images/Console.Flashcards/StudySession.png)|

## Study History

- The program offers the ability to view coding session history
  
 |Study History|
 |:-:|
 |![Study History](https://rvnprojectstorage.blob.core.windows.net/images/Console.Flashcards/StudyHistory.png)|
