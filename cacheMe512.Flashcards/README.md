
Flashcards App
==============

A simple console-based flashcards application for learning and memorization. 
Users can create stacks of flashcards,review them in study sessions, 
and track their progress.

---

## Features

- ✅ Create, edit, and delete **stacks of flashcards**
- ✅ Start new or continue **study sessions**
- ✅ Track session history with **scores and completion details**
- ✅ Organized **sorting and filtering** for study sessions  

---

## Installation

### 1️⃣ Clone the Repository

```sh
git clone https://github.com/yourusername/flashcards-app.git
cd flashcards-app
```

### 2️⃣ Build the Project

Ensure you have the **.NET SDK** installed, then run:

```sh
dotnet build
```

### 3️⃣ Configure Database

The app uses a SQL database. Configure your connection in `appsettings.json`:

```json
"ConnectionStrings": {
  "FlashcardsDatabase": "your-database-connection-string"
}
```

### 4️⃣ Run the Application

```sh
dotnet run
```

---

## 🛠 Usage

1. **Manage Stacks**: Create and organize flashcard groups  
2. **Add Flashcards**: Add questions & answers to each stack  
3. **Study Sessions**: Review flashcards and track your score  
4. **View History**: Analyze past session performance  
