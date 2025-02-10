# Flashcard Application

## Introduction
The flashcard application is a C# console application designed to facilitate studying using flashcards. This project allows users to create, view, and study custom flashcard stacks.
 
# Overview
## This application enables users to:

Manage Stacks: Create stacks of flashcards where each stack has a unique name.
Manage Flashcards: Create flashcards that belong to a specific stack. Flashcards are displayed to the user using DTOs that hide internal identifiers.
Study Sessions: Conduct study sessions based on selected stacks. Each session records the date and score, and all sessions are stored in the database.
Key points:

Every flashcard must be part of a stack.
If a stack is deleted, all its associated flashcards and study sessions are also deleted.
When displaying a stack to the user, flashcard IDs are renumbered consecutively (e.g., if flashcard #5 is deleted from a set of 10, the displayed IDs will run from 1 to 9).

 
## Contributing
Contributions are welcome! Please fork this repository and submit a pull request for any improvements or bug fixes.

## License
This project does not currently specify a license.

 
 