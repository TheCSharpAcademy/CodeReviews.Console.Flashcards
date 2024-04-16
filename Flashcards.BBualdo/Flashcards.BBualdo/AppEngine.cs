using DatabaseLibrary;
using DatabaseLibrary.Models;
using Spectre.Console;

namespace Flashcards.BBualdo;

internal class AppEngine
{
  public bool IsRunning { get; set; }
  public DbContext DbContext { get; set; }

  public AppEngine()
  {
    IsRunning = true;
    DbContext = new DbContext();
  }

  public void MainMenu()
  {
    AnsiConsole.Clear();
    ConsoleEngine.ShowAppTitle();
    ConsoleEngine.ShowMenuTitle("Main Menu");

    string userChoice = ConsoleEngine.MenuSelector("What you want to do?", ["Manage Flashcards", "Study Sessions", "Quit"]);

    switch (userChoice)
    {
      case "Quit":
        AnsiConsole.Markup("[blue]Goodbye![/]\n");
        IsRunning = false;
        break;
      case "Manage Flashcards":
        FlashcardsMenu();
        break;
      case "Study Sessions":
        StudySessionsMenu();
        break;
    }
  }

  public void FlashcardsMenu()
  {
    AnsiConsole.Clear();
    ConsoleEngine.ShowAppTitle();
    ConsoleEngine.ShowMenuTitle("Flashcards Menu");

    string userChoice = ConsoleEngine.MenuSelector("", ["Back", "Manage Stacks", "Show Flashcards", "Create Flashcard", "Update Flashcard", "Delete Flashcard"]);

    switch (userChoice)
    {
      case "Back":
        MainMenu();
        break;
      case "Manage Stacks":
        StacksMenu();
        break;
      case "Show Flashcards":
        ShowFlashcards();
        break;
      case "Create Flashcard":
        CreateFlashcard();
        break;
      case "Update Flashcard":
        UpdateFlashcard();
        break;
      case "Delete Flashcard":
        DeleteFlashcard();
        break;
    }
  }

  public void StacksMenu()
  {
    AnsiConsole.Clear();
    ConsoleEngine.ShowAppTitle();
    ConsoleEngine.ShowMenuTitle("Stacks Menu");

    string userChoice = ConsoleEngine.MenuSelector("", ["Back", "Show Stacks", "Create Stack", "Update Stack", "Delete Stack"]);

    switch (userChoice)
    {
      case "Back":
        FlashcardsMenu();
        break;
      case "Show Stacks":
        ShowStacks();
        break;
      case "Create Stack":
        CreateStack();
        break;
      case "Update Stack":
        UpdateStack();
        break;
      case "Delete Stack":
        DeleteStack();
        break;
    }
  }

  public void StudySessionsMenu()
  {
    AnsiConsole.Clear();
    ConsoleEngine.ShowAppTitle();
    ConsoleEngine.ShowMenuTitle("Study Sessions Menu");

    string userChoice = ConsoleEngine.MenuSelector("", ["Back", "Study", "Show Study Sessions"]);

    switch (userChoice)
    {
      case "Back":
        MainMenu();
        break;
      case "Study":
        StartStudySession();
        break;
      case "Show Study Sessions":
        GetStudySessions();
        break;
    }
  }

  #region Stacks Methods
  private void ShowStacks()
  {
    List<Stack> stacks = DbContext.StacksAccess.GetStacksList();
    DbContext.StacksAccess.GetAllStacks(stacks);

    PressAnyKey();
  }

  private void CreateStack()
  {
    List<Stack> stacks = DbContext.StacksAccess.GetStacksList();
    string? stackName = UserInput.GetStackName(stacks);
    if (stackName == null) { StacksMenu(); return; }

    DbContext.StacksAccess.InsertStack(stackName);

    PressAnyKey();
  }

  private void UpdateStack()
  {
    List<Stack> stacks = DbContext.StacksAccess.GetStacksList();
    if (DbContext.StacksAccess.GetAllStacks(stacks))
    {
      int? stackId = UserInput.GetStackId(stacks);
      if (stackId == null) { StacksMenu(); return; }

      string? name = UserInput.GetStackName(stacks);
      if (name == null) { StacksMenu(); return; }

      DbContext.StacksAccess.UpdateStack(stackId, name);
    }

    PressAnyKey();
  }

  private void DeleteStack()
  {
    List<Stack> stacks = DbContext.StacksAccess.GetStacksList();
    if (DbContext.StacksAccess.GetAllStacks(stacks))
    {
      int? stackId = UserInput.GetStackId(stacks);
      if (stackId == null) { StacksMenu(); return; }

      DbContext.StacksAccess.DeleteStack(stackId);
    }

    PressAnyKey();
  }
  #endregion

  #region Flashcards Methods
  private void ShowFlashcards()
  {
    List<Stack> stacks = DbContext.StacksAccess.GetStacksList();
    if (DbContext.StacksAccess.GetAllStacks(stacks))
    {
      int? stackId = UserInput.GetStackId(stacks);
      if (stackId == null) { FlashcardsMenu(); return; }

      List<FlashcardDTO> flashcards = DbContext.FlashcardsAccess.GetFlashcardsList(stackId);
      DbContext.FlashcardsAccess.GetAllFlashcards(flashcards);
    }

    PressAnyKey();
  }

  private void CreateFlashcard()
  {
    List<Stack> stacks = DbContext.StacksAccess.GetStacksList();
    if (DbContext.StacksAccess.GetAllStacks(stacks))
    {
      int? stackId = UserInput.GetStackId(stacks);
      if (stackId == null) { FlashcardsMenu(); return; }

      string? question = UserInput.GetQuestion();
      if (question == null) { FlashcardsMenu(); return; }

      string? answer = UserInput.GetAnswer();
      if (answer == null) { FlashcardsMenu(); return; }

      DbContext.FlashcardsAccess.InsertFlashcard(stackId, question, answer);
    }

    PressAnyKey();
  }

  private void UpdateFlashcard()
  {
    List<Stack> stacks = DbContext.StacksAccess.GetStacksList();
    if (DbContext.StacksAccess.GetAllStacks(stacks))
    {
      int? stackId = UserInput.GetStackId(stacks);
      if (stackId == null) { FlashcardsMenu(); return; }

      List<FlashcardDTO> flashcards = DbContext.FlashcardsAccess.GetFlashcardsList(stackId);
      DbContext.FlashcardsAccess.GetAllFlashcards(flashcards);

      int? flashcardId = UserInput.GetFlashcardId(flashcards);
      if (flashcardId == null) { FlashcardsMenu(); return; }

      int? newStackId = UserInput.GetNewStackIdForFlashcard(stacks);
      if (newStackId == null) { FlashcardsMenu(); return; }

      string? question = UserInput.GetQuestion();
      if (question == null) { FlashcardsMenu(); return; }

      string? answer = UserInput.GetAnswer();
      if (answer == null) { FlashcardsMenu(); return; }

      DbContext.FlashcardsAccess.UpdateFlashcard(stackId, flashcardId, newStackId, question, answer);
    }

    PressAnyKey();
  }

  private void DeleteFlashcard()
  {
    List<Stack> stacks = DbContext.StacksAccess.GetStacksList();
    if (DbContext.StacksAccess.GetAllStacks(stacks))
    {
      int? stackId = UserInput.GetStackId(stacks);
      if (stackId == null) { FlashcardsMenu(); return; }

      List<FlashcardDTO> flashcards = DbContext.FlashcardsAccess.GetFlashcardsList(stackId);
      DbContext.FlashcardsAccess.GetAllFlashcards(flashcards);

      int? flashcardId = UserInput.GetFlashcardId(flashcards);
      if (flashcardId == null) { FlashcardsMenu(); return; }

      DbContext.FlashcardsAccess.DeleteFlashcard(flashcardId);

      UpdateFlashcardsIDs(stackId);
    }

    PressAnyKey();
  }

  private void UpdateFlashcardsIDs(int? stackId)
  {
    List<FlashcardDTO> remainingFlashcards = DbContext.FlashcardsAccess.GetFlashcardsList(stackId);

    if (remainingFlashcards.Count == 0) return;

    for (int i = 0; i < remainingFlashcards.Count; i++)
    {
      int newId = i + 1;
      DbContext.FlashcardsAccess.UpdateFlashcardId(stackId, remainingFlashcards[i].Display_Id, newId);
    }
  }
  #endregion

  #region Study Sessions Methods
  private void GetStudySessions()
  {
    List<StudySession> sessions = DbContext.SessionAccess.GetStudySessionsList();
    DbContext.SessionAccess.GetAllStudySessions(sessions);

    PressAnyKey();
  }

  private void StartStudySession()
  {
    List<Stack> stacks = DbContext.StacksAccess.GetStacksList();
    if (DbContext.StacksAccess.GetAllStacks(stacks))
    {
      int? stackId = UserInput.GetStackId(stacks);
      if (stackId == null) return;
      StartStudying(stackId);
    }

    PressAnyKey();
  }

  private void StartStudying(int? stackId)
  {
    StudySession session = new StudySession(DateTime.Now, stackId);

    List<FlashcardDTO> flashcards = DbContext.FlashcardsAccess.GetFlashcardsList(stackId);
    List<FlashcardDTO> flashcardsPool = [.. flashcards];
    Random random = new Random();

    for (int i = 0; i < flashcards.Count; i++)
    {
      AnsiConsole.Clear();

      int flashcardNumber = random.Next(0, flashcardsPool.Count);
      string question = flashcardsPool[flashcardNumber].Question;
      string userAnswer = AnsiConsole.Ask<string>($"Translate to English '[yellow]{question}[/]': ");

      if (userAnswer.ToLower() == flashcardsPool[flashcardNumber].Answer.ToLower())
      {
        session.Score++;
        AnsiConsole.Markup("[green]Correct answer![/] [blue]Press any key to continue.[/] ");
        Console.ReadKey();
      }
      else
      {
        AnsiConsole.Markup($"[red]Incorrect answer![/] Correct answer is: [green]{flashcardsPool[flashcardNumber].Answer}[/]. [blue]Press any key to continue.[/] ");
        Console.ReadKey();
      }

      flashcardsPool.Remove(flashcardsPool[flashcardNumber]);
    }

    session.Score = Convert.ToInt32((double)session.Score / flashcards.Count * 100);

    AnsiConsole.Markup($"That's it! You answered correctly for [green]{session.Score}%[/] of Flashcard's questions. ");

    DbContext.SessionAccess.InsertSession(session);
  }
  #endregion

  private void PressAnyKey()
  {
    AnsiConsole.Markup("\n\n[blue]Press any key to return to Main Menu.[/]");
    Console.ReadKey();
  }
}