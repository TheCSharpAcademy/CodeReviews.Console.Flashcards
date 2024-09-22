<h1>Flashcards App</h1>
<p>This Flashcards application allows users to create, view, update, delete, and study stacks of flashcards.</p>
<p>Additionally, it stores study session scores for tracking progress.</p>

<h2>Table Of Content</h2>
<ol>
  <li><a href="#features">Features</a></li>
  <li><a href="#technologies">Technologies</a></li>
  <li><a href="#database-setup">Database Setup</a></li>
  <li><a href="#usage">Usage</a></li>
</ol>

<h2>Features</h2>
  <ul>
    <li>Create, view, update, and delete flashcard stacks.</li>
    <li>Add and edit flashcards in each stack.</li>
    <li>Track study session performance and scores.</li>
    <li>Automatically renumber flashcards within a stack when updated.</li>
    <li>View historical study session data.</li>
  </ul>
<h2>Technologies</h2>
  <ul>
    <li>.NET Core 8.0: The framework used for the backend logic.</li>
    <li>SQL Server (LocalDB): For database management.</li>
    <li>Spectre.Console: For colorful and interactive console UI.</li>
    <li>SQL Server Management Studio (SSMS): For database management.</li>
  </ul>

<h2>Database Setup</h2>
<p>The app uses SQL Server LocalDB to store flashcards, stacks, and study session data.</p>

<h3>Automatic Database Creation</h3>
<p>Upon launching the app for the first time, the necessary database and tables will be created automatically in the project's bin/Debug/net8.0 folder.</p>

<h3>Manual Database Setup (optional)</h3>
<p>If you want to set up the database manually:</p>
  <ol>
    <li>Open SQL Server Management Studio (SSMS).</li>
    <li>Connect to the (LocalDB)\MSSQLLocalDB instance.</li>
    <li>Navigate to the Attach Databases option and attach the flashcardsdb.mdf located in the project’s bin/Debug/net8.0 folder.</li>
  </ol>
<h3>Tables Structure</h3>
  <ul>
    <li>Stacks: Stores information about each flashcard stack.</li>
    <li>Flashcards: Contains the flashcards associated with each stack.</li>
    <li>StudySessions: Records each study session's date, score, and associated stack.</li>
  </ul>
<h2>Usage</h2>
<h3>Main Menu Options:</h3>
  <ul>
    <li>Create Stack: Add a new flashcard stack.</li>
    <li>Update Stack: Rename an existing stack and renumber the flashcards.</li>
    <li>View Stacks: View all stacks and their associated flashcards.</li>
    <li>Delete Stack: Permanently remove a stack and its flashcards.</li>
    <li>Study Session: Select a stack, go through the flashcards, and record a score.</li>
    <li>View Study Sessions: See a list of past study sessions and scores.</li>
    <li>Close App: Exit the application.</li>
  </ul>
<h3>Sample Workflow:</h3>
  <ol>
    <li>Create a new stack (e.g., "Mathematics").</li>
    <li>Add flashcards (e.g., "What is 2 + 2?", "Answer: 4").</li>
    <li>Start a study session and answer the flashcards.</li>
    <li>View your study session scores.</li>
  </ol>
