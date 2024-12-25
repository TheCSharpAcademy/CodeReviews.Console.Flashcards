# Flashcard Management Application

This is a console-based application to manage flashcards and study sessions using C#, SQL Server, and Spectre.Console for a user-friendly terminal interface.

---

## Features

- **Flashcard Management**:
  - Add, update, view, and delete flashcards.
  - Organize flashcards into named stacks.
- **Study Sessions**:
  - Track study sessions with date and scores.
  - Retrieve and display historical session data.
- **Interactive Console UI**:
  - Tables and grids rendered with Spectre.Console for enhanced readability.
- **Database Integration**:
  - Uses SQL Server for persistent storage.
  - Implements CRUD operations with parameterized SQL commands.

---

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [Visual Studio Code](https://code.visualstudio.com/) (Optional but recommended)
- [Spectre.Console](https://spectreconsole.net/) NuGet package

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/yourusername/flashcard-app.git
   cd flashcard-app
   ```

2. Install dependencies:

   ```bash
   dotnet restore
   ```

3. Update the SQL server name in App.config:

```bash
<add key="ServerName" value="<SERVER NAME HERE>" />
```

4. Run the app:

   ```bash
   dotnet run
   ```
