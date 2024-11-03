# Flashcards Console Application

I have spent countless hours on this project over the last 6 months. I have
started and restarted this project, spent hours online researching each and
every bit, and finally got help from a C# Dev who is now mentoring me on
improving my skills and best practices. This is by no means a perfect final
solution, but I bellieve it is functional and meets the project requirements.

A console-based application for organizing and studying flashcards in
structured stacks. This project leverages C# with SQL Server and Entity
Framework Core to handle data management, enabling users to manage flashcards,
conduct study sessions, and review their study progress.

## Given Requirements

- [x] The application organizes flashcards into named stacks.
- [x] Each stack has a unique name, and deleting a stack deletes all associated
  flashcards and study sessions.
- [x] Flashcards are presented to users without stack IDs, ensuring a clean
  display.
- [x] Flashcard IDs appear sequentially without gaps when viewed, even if some
  flashcards have been deleted.
- [x] Users can create study sessions for any stack, recording the date and
  session score automatically.
- [x] Study sessions cannot be modified or deleted after creation.
- [x] Detailed session history is available to track progress over time.

## Features

### SQL Server Database Connection

- The program uses SQL Server for persistent storage.
- Tables for stacks, flashcards, and study sessions are created and managed
  automatically using Entity Framework Core.
- **Database Setup Logic**: The `SetupDatabase` helper method simplifies
  connection configuration, prompting users to set up or test the database
  connection seamlessly.

### Console-Based UI for Seamless Navigation and Interaction

- Users interact through a text-based menu, allowing for smooth navigation
  between stack and flashcard management, study sessions, and configuration
  settings.
- Modularized structure with separate menus for study sessions, options, and
  reports enhances usability and code organization.

### CRUD Operations for Stacks and Flashcards

- Users can create, view, update, and delete stacks and flashcards with data
  validation to ensure meaningful inputs.
- Deleting a stack removes all associated flashcards and study sessions
  automatically.
- Flashcards are renumbered sequentially within each stack for a consistent
  display.

### Study Sessions and Progress Tracking

- Users can initiate study sessions where they answer questions from a selected
  stack.
- Sessions record the score and date automatically, and all past sessions can
  be viewed with associated statistics.
- Progress tracking with filters by date or score range to aid in reviewing
  specific study sessions.

### Enhanced Display and Feedback with Spectre.Console

- The application provides user-friendly output with Spectre.Console, enabling
  color-coded feedback and structured data display.
- Reports are neatly formatted to show session history, scores, and stack
  summaries.

## Challenges

- **Data Validation**: Ensuring valid data input (e.g., non-empty stack names)
  and implementing sequential flashcard numbering.
- **Entity Framework Core**: Leveraging EF Core to manage data relationships
  required understanding configuration and performance optimizations.
- **DTOs for User-Friendly Views**: Implementing DTOs improved user experience
  by allowing flashcards to be displayed without internal identifiers.
- **Console UI Development**: Designing an intuitive console interface with
  error handling and navigation took careful planning and testing.

## Lessons Learned

- **Modular Code Structure**: Splitting functionality into distinct services,
  menus, and helpers improved code readability and maintainability.
- **Effective Use of DTOs**: Using DTOs for simplified data display streamlined
  the user interface.
- **Entity Framework Mastery**: Managing relationships, cascading deletes, and
  data validation in Entity Framework helped streamline database management.

## Areas to Improve

- **Enhanced Error Handling**: Broader validation for edge cases, such as date
  formatting and score ranges, could improve robustness.
- **User Analytics**: Additional reports to show cumulative progress over time,
  such as average scores and session counts per stack.
- **Export Data**: Adding functionality to export session data for external use
  or backup.

## Resources Used

- Entity Framework Core documentation
- Spectre.Console documentation
- [StackOverflow articles](https://stackoverflow.com/) and community
  discussions
- Mentor guidance on data structure design and user interface
