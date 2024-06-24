# FlashCards App

## Summary

Flashcards App helps users learn effectively by allowing them to undertake
study sessions where they can practice with flash cards and track their
study session scores. A flashcard is a learning tool where information
is presented as a question or term on one side, with its corresponding answer
on the reverse side, used for quick study and recall.

## Feature overview

- Conduct study sessions
- View past study sessions
- Manage stacks and flashcards
- Create new stacks
- View average score report per month per stack

## Run application locally

### Pre-requisites & Configuration

- SQL server local DB must be running locally
- Database connection configurable via `<Root>/FlashcardsProgram/appsettings.json`

### Steps to run

- Clone this repository and `cd` into it
- If required, change `appsettings.json` values
- `cd FlashcardsProgram && dotnet run`

## Tech stack

- C#
- SQL server
- Dapper ORM

## Database details

- When the app starts, it connects to the local SQL server specified in `appsettings.json`
- A database will be automatically created with the name specified in `appsettings.json`
  (if it doesn't exist)
- Tables will be automatically created if they don't exist
