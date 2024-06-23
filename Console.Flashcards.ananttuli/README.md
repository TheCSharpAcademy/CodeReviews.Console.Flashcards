# FlashCards App

## Summary

Flashcards App helps users learn effectively by allowing them to undertake study
sessions where they can practice with flash cards and track their scores.

## Run application locally

### Pre-requisites & Configuration

- SQL server local DB must be running locally
- Database connection configurable via `<Root>/FlashcardsProgram/appsettings.json`

### Steps to run

- Clone this repository and `cd` into it
- `cd FlashcardsProgram && dotnet run`

## Tech stack

- SQL server
- Dapper
- C#

## Database details

- When the app starts, it connects to the local SQL server specified in `appsettings.json`
- A database will be automatically created with the name specified in `appsettings.json`
  (if it doesn't exist)
- Tables will be automatically created if they don't exist
