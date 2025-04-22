# Flashcard Console Application 📚

A C# console application for managing and studying flashcards, built with a focus on simplicity and functionality. This application allows you to create, organize, and study flashcards grouped into stacks. You can track your study sessions and analyze your progress over time.

---

## Features 🚀

1. **Flashcard Management**:
    - Group flashcards into stacks (e.g., "C# Basics").
    - Create flashcards with a title and content (front/back sides).

2. **Study Sessions**:
    - Study your flashcards interactively using the console.
    - Track your performance and score in each session.

3. **Session Analytics**:
    - View all your study sessions.
    - Analyze sessions aggregated by month.
    - Calculate the average score of all your sessions.

4. **Spectre.Console UI**:
    - The application uses Spectre.Console for a rich and intuitive UI experience.
    - Navigate using arrow keys for seamless interaction.

---

## Getting Started 🛠️

### Prerequisites

- **Docker** and **Docker Compose** installed on your machine.
- Basic knowledge of Docker commands.

### Setting Up the Application

1. Clone the repository:
   ```bash
   git clone https://github.com/KamilKolanowski/CodeReviews.Console.Flashcards.git
   cd Flashcards.KamilKolanowski
   ```

2. Start the application and create the database with all necessary tables:
   ```bash
   docker-compose up --build
   ```

   This command will build the application, set up the required services, and initialize the database.

3. If you want to bulk insert temporary data into the database for testing, you can run the `BulkInsert.sql` script:
    - The script is located in the `Data/DatabaseFiles` directory.

---

## How to Use the Application 🎮

1. **Launching the App**:
    - Once the `docker-compose` command completes, the application will be ready to use.
    - Interact with the app via the console interface using arrow keys.

2. **Creating Flashcards**:
    - Organize flashcards into stacks (e.g., "C# Basics").
    - Add individual flashcards with a title and front/back content.

3. **Studying Flashcards**:
    - Choose a stack to study from.
    - Track your performance and scores during study sessions.

4. **Viewing Analytics**:
    - Review your study sessions.
    - Analyze monthly session counts and calculate the average score.

---

## File Structure 📂

```
.
├── Data
│   ├── DatabaseFiles
│   │   └── BulkInsert.sql   # Script to populate database with temporary data
├── ...
├── compose.yaml       # Docker Compose configuration
├── Program.cs         # Main application source code      
└── README.md          # Project documentation
```

---

## Built With 🛠

- **C#** - Core application logic.
- **Spectre.Console** - Console UI framework.
- **Docker/Docker Compose** - Simplified setup and deployment.
- **SQL Server** - Database for storing flashcards, stacks and study session data.

---

## Contributing 🤝

Feel free to contribute to this project by submitting pull requests or reporting issues.

---

Enjoy studying with your personalized flashcard manager! 🚀