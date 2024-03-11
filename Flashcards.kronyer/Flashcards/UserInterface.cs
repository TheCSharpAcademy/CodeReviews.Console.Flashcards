using Flashcards.Models;
using Microsoft.IdentityModel.Tokens;
using Spectre.Console;
using static Flashcards.Enums;

namespace Flashcards
{
    internal class UserInterface
    {
        public static void MainMenu()
        {
            bool isRunning = true;

            while (isRunning)
            {
                var usersChoice = AnsiConsole.Prompt(new SelectionPrompt<MainMenuChoices>().Title("Pick your choice").AddChoices(MainMenuChoices.ManageStacks, MainMenuChoices.ManageFlashcards, MainMenuChoices.StudySession, MainMenuChoices.StudyHistory, MainMenuChoices.Quit));

                switch (usersChoice)
                {
                    case MainMenuChoices.ManageStacks:
                        StacksMenu();
                        break;
                    case MainMenuChoices.ManageFlashcards:
                        FlashcardsMenu();
                        break;
                    case MainMenuChoices.StudySession:
                        StudySession();
                        break;
                    case MainMenuChoices.StudyHistory:
                        ShowStudyHistory();
                        break;
                    case MainMenuChoices.Quit:
                        Console.WriteLine("Farewell!");
                        isRunning = false;
                        break;
                }
            }
        }

        private static void ShowStudyHistory()
        {
            var dataAccess = new DataAcess();
            var sessions = dataAccess.GetStudySessionData();

            var table = new Table();
            table.AddColumn("Date");
            table.AddColumn("Stack");
            table.AddColumn("Result");
            table.AddColumn("Percentage");
            table.AddColumn("Duration");

            foreach (var session in sessions)
            {
                table.AddRow(session.Date.ToShortDateString(), session.StackName, $"{session.CorrectAnswers} out of {session.Questions}", $"{session.Percentage}%", session.Time.ToString());
            }
            AnsiConsole.Write(table);
        }

        private static void StacksMenu()
        {
            bool isRunning = true;

            while (isRunning)
            {
                var usersChoice = AnsiConsole.Prompt(new SelectionPrompt<StackChoices>().Title("Chose an option").AddChoices(StackChoices.ViewStacks, StackChoices.AddStack, StackChoices.UpdateStack, StackChoices.DeleteStack, StackChoices.Return));

                switch (usersChoice)
                {
                    case StackChoices.ViewStacks:
                        ViewStacks();
                        break;
                    case StackChoices.AddStack:
                        AddStack();
                        break;
                    case StackChoices.DeleteStack:
                        DeleteStack();
                        break;
                    case StackChoices.UpdateStack:
                        UpdateStack();
                        break;
                    case StackChoices.Return:
                        isRunning = false;
                        break;
                }
            }
        }

        private static int ChooseStack(string message)
        {
            var dataAccess = new DataAcess();
            var stacks = dataAccess.GetAllStacks();

            var stacksArray = stacks.Select(x => x.Name).ToArray();
            var option = AnsiConsole.Prompt(new SelectionPrompt<string>().Title(message).AddChoices(stacksArray));

            var stackId = stacks.Single(x => x.Name == option).Id;
            return stackId;
        }

        private static void ViewStacks()
        {
            Console.Clear();
            var dataAcc = new DataAcess();
            var stacks = dataAcc.GetAllStacks();

            var table = new Table();
            table.AddColumn("Stacks");

            foreach (var stack in stacks)
            {
                table.AddRow(new Markup($"[underline]{stack.Name}[/]"));
            }
            table.Width = 20;

            AnsiConsole.Write(table);
        }

        private static void AddStack()
        {
            Stack stack = new();

            stack.Name = AnsiConsole.Ask<string>("Inser Stack's Name, or type 0 to return:");
            if (stack.Name == "0")
            {
                Console.Clear();
                StacksMenu();
            }

            while (string.IsNullOrEmpty(stack.Name))
            {
                stack.Name = AnsiConsole.Ask<string>("Stack name can't be empty...");
                if (stack.Name == "0")
                {
                    Console.Clear();
                    StacksMenu();
                }
            }

            var dataAccess = new DataAcess();
            dataAccess.InsertStack(stack);
            Console.Clear();
            ViewStacks();
        }

        private static void DeleteStack()
        {
            var dataAccess = new DataAcess();

            Console.Clear();
            if (dataAccess.GetAllStacks().IsNullOrEmpty())
            {
                Console.WriteLine("Nothing to delete...");
            }
            else
            {
                var id = ChooseStack("Chose stack to delete");

                if (!AnsiConsole.Confirm("Are you sure?"))
                {
                    Console.Clear();
                    return;
                }
                dataAccess.DeleteStack(id);
                Console.Clear();
            }
        }

        private static void UpdateStack()
        {
            var dataAccess = new DataAcess();

            if (dataAccess.GetAllStacks().IsNullOrEmpty())
            {
                Console.WriteLine("Nothing to delete...");
            }
            else
            {
                Console.Clear();
                var stack = new Stack();
                stack.Id = ChooseStack("Chose one stack to edit");
                stack.Name = AnsiConsole.Ask<string>("Insert Stack's new name");

                dataAccess.UpdateStack(stack);
                Console.Clear();
            }
        }

        private static void FlashcardsMenu()
        {
            bool isRunning = true;

            while (isRunning)
            {
                var usersChoice = AnsiConsole.Prompt(new SelectionPrompt<FlashcardChoices>().Title("Choose an option").AddChoices(FlashcardChoices.ViewFlashcards, FlashcardChoices.AddFlashcard, FlashcardChoices.UpdateFlashcard, FlashcardChoices.DeleteFlashcard, FlashcardChoices.Return));

                switch (usersChoice)
                {
                    case FlashcardChoices.ViewFlashcards:
                        ViewFlashcards();
                        break;
                    case FlashcardChoices.AddFlashcard:
                        AddFlashcard();
                        break;
                    case FlashcardChoices.DeleteFlashcard:
                        DeleteFlashcard();
                        break;
                    case FlashcardChoices.UpdateFlashcard:
                        UpdateFlashcard();
                        break;
                    case FlashcardChoices.Return:
                        isRunning = false;
                        break;
                }
            }
        }

        private static int ChooseFlashcard(string message, int stackId)
        {
            var dataAccess = new DataAcess();
            var flashcards = dataAccess.GetFlashcards(stackId);

            var flashcardsArray = flashcards.Select(x => x.Question).ToArray();
            var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title(message)
                .AddChoices(flashcardsArray));

            var flashcardId = flashcards.Single(x => x.Question == option).Id;

            return flashcardId;
        }

        static void UpdateFlashcard()
        {
            Console.Clear();
            var stackId = ChooseStack("Choose the stack where flashcard is");
            var flashcardId = ChooseFlashcard("Choose flashcard to update", stackId);

            var propertiesToUpdate = new Dictionary<string, object>();

            if (AnsiConsole.Confirm("Would you like to update this question?"))
            {
                var question = GetQuestion();
                propertiesToUpdate.Add("Question", question);
            }

            if (AnsiConsole.Confirm("Would you like to update this answer?"))
            {
                var answer = GetAnswer();
                propertiesToUpdate.Add("Answer", answer);
            }

            if (AnsiConsole.Confirm("Would you like to update this stack?"))
            {
                var stack = ChooseStack("Choose a new stack for the flashcard");
                propertiesToUpdate.Add("StackId", stack);
            }
            if (propertiesToUpdate.Values.IsNullOrEmpty())
            {
                Console.Clear();
                FlashcardsMenu();
            }
            var dataAccess = new DataAcess();
            dataAccess.UpdateFlashcard(flashcardId, propertiesToUpdate);
            Console.Clear();
        }

        static void DeleteFlashcard()
        {
            var dataAccess = new DataAcess();

            Console.Clear();

            var stackId = ChooseStack("Where is the flashcard you want to delete?");
            if (dataAccess.GetFlashcards(stackId).IsNullOrEmpty())
            {
                Console.WriteLine("Nothing to delete...");
                Console.ReadKey();
                MainMenu();
            }
            else
            {
                var flashcard = ChooseFlashcard("Choose a flashcard to delete", stackId);

                if (!AnsiConsole.Confirm("Are you sure?"))
                {
                    return;
                }

                dataAccess.DeleteFlashcard(flashcard);

                Console.Clear();
            }
        }

        static void AddFlashcard()
        {
            Console.Clear();
            Flashcard flashcard = new Flashcard();

            flashcard.StackId = ChooseStack("Choose a stack");
            //selecionou o bonitao
            flashcard.Question = GetQuestion();

            flashcard.Answer = GetAnswer();

            var dataAccess = new DataAcess();
            dataAccess.InsertFlashcard(flashcard);
            Console.Clear();

        }

        static void ViewFlashcards()
        {
            Console.Clear();
            var dataAccess = new DataAcess();
            var stackId = ChooseStack("Choose a stack");
            var flashcards = dataAccess.GetFlashcards(stackId);
            var stack = dataAccess.GetStackByID(stackId);

            var table = new Table();

            table.AddColumn($"{stack.Name}");

            int i = 1;
            foreach (var flashcard in flashcards)
            {
                table.AddRow($"{i} {flashcard.Question} {flashcard.Answer}");
                i++;
            }
            AnsiConsole.Write(table);
        }

        private static string GetQuestion()
        {
            var question = AnsiConsole.Ask<string>("Insert Question");

            while (string.IsNullOrEmpty(question))
            {
                question = AnsiConsole.Ask<string>("Question can't be empty...");
            }

            return question;
        }

        private static string GetAnswer()
        {
            var answer = AnsiConsole.Ask<string>("Insert Answer");

            while (string.IsNullOrEmpty(answer))
            {
                answer = AnsiConsole.Ask<string>("Answer cant' be empty");
            }
            return answer;
        }

        internal static void StudySession()
        {
            var id = ChooseStack("Choose stack to study");

            var dataAccess = new DataAcess();
            var flashcards = dataAccess.GetFlashcards(id);

            var studySession = new StudySession();
            studySession.Questions = flashcards.Count();
            studySession.StackId = id;
            studySession.Date = DateTime.Now;

            studySession.CorrectAnswers = 0;

            foreach (var flashcard in flashcards)
            {
                var answer = AnsiConsole.Ask<string>($"{flashcard.Question}: ");

                while (string.IsNullOrEmpty(answer))
                {
                    answer = AnsiConsole.Ask<string>($"Answer can't be empty... {flashcard.Question}:");
                }

                if (string.Equals(answer.Trim(), flashcard.Answer, StringComparison.OrdinalIgnoreCase))
                {
                    studySession.CorrectAnswers++;
                    Console.WriteLine("Correct\n");
                }
                else
                {
                    Console.WriteLine($"You're wrong, the answer is {flashcard.Answer}\n");
                }
            }

            Console.WriteLine($"You've got {studySession.CorrectAnswers} out of {flashcards.Count()}");

            studySession.Time = DateTime.Now - studySession.Date;

            dataAccess.InsertStudySession(studySession);
        }
    }
}
