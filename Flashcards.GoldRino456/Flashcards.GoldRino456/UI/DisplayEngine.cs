using Flashcards.GoldRino456.Database.Models;
using Spectre.Console;
using Utilities.GoldRino456;

namespace Flashcards.GoldRino456.UI
{
    internal static class DisplayEngine
    {
        #region Universal Display Methods

        public static void DisplayErrorToUser(string error)
        {
            AnsiConsole.MarkupLine("[red]" + error + "[/]");
        }

        public static void ClearScreen()
        {
            AnsiConsole.Clear();
        }

        public static void PressAnyKeyToContinue()
        {
            AnsiConsole.WriteLine("Press any key to continue...");
            AnsiConsole.Console.Input.ReadKey(false);
        }

        #endregion

        #region Menu Display Methods

        public static MenuOptions DisplayMainMenu()
        {
            var menuOptions = new Dictionary<string, int> {
                { "Start A Study Session", ((int)MenuOptions.StartStudy) },
                { "Edit Study Materials", ((int)MenuOptions.EditMaterials) },
                { "View Study Materials", ((int)MenuOptions.ViewMaterials) },
                { "View Study Session History", ((int)MenuOptions.ViewStudySession) },
                { "Quit", ((int)MenuOptions.Quit) }};

            return (MenuOptions)DisplayUtils.PromptUserForIndexSelection("What would you like to do?", menuOptions);
        }

        public static EditOptions DisplayEditMenu()
        {
            var editOptions = new Dictionary<string, int> {
                { "Create A New Stack", ((int)EditOptions.CreateStack) },
                { "Create A New Flashcard", ((int)EditOptions.CreateFlashcard) },
                { "Edit An Existing Stack", ((int)EditOptions.EditStack) },
                { "Edit An Existing Flashcard", ((int)EditOptions.EditFlashcard) },
                { "Delete a Stack", ((int)EditOptions.DeleteStack) },
                { "Delete a Flashcard", ((int)EditOptions.DeleteFlashcard) },
                { "Go Back", ((int)EditOptions.Quit) }};

            return (EditOptions)DisplayUtils.PromptUserForIndexSelection("Please make a selection:", editOptions);
        }


        #endregion

        #region Stack Display Methods
        public static void PromptUserForStackInfo(Stack stack, List<string> stackNames)
        {
            while (true)
            {
                var stackName = DisplayUtils.PromptUserForStringInput("What do you want to call this card stack?");

                if (!stackNames.Contains(stackName, StringComparer.OrdinalIgnoreCase))
                {
                    stack.StackName = stackName;
                    return;
                }
                else
                {
                    DisplayErrorToUser("Stack name is already taken. Please enter a different stack name.");
                }
            }
        }

        public static int PromptUserForStackSelection(string prompt, List<Stack> stackList)
        {
            var stackChoices = new Dictionary<string, int>();

            int index = 0;
            foreach (Stack stack in stackList)
            {
                stackChoices[stack.StackName] = index;
                index++;
            }

            return DisplayUtils.PromptUserForIndexSelection(prompt, stackChoices);
        }

        #endregion

        #region Flashcard Display Methods

        public static void PromptUserForFlashcardInfo(Flashcard flashcard, List<Stack> stackList)
        {
            var frontOfCard = DisplayUtils.PromptUserForStringInput("Please enter the text for the front side of the flashcard: ");
            var backOfCard = DisplayUtils.PromptUserForStringInput("Please enter the text for the back side of the flashcard: ");
            var stackIndex = PromptUserForStackSelection("Which stack should this card be added to?", stackList);

            flashcard.StackId = stackList[stackIndex].StackId;
            flashcard.FrontOfCard = frontOfCard;
            flashcard.BackOfCard = backOfCard;
        }

        public static int PromptUserForFlashcardSelection(string prompt, List<Flashcard> cardList)
        {
            DisplayFlashcards(cardList);
            return DisplayUtils.PromptUserForIntegerInput(prompt, 1, cardList.Count + 1) - 1;
        }

        public static void DisplayFlashcards(List<Flashcard> flashcards)
        {
            int index = 1;
            var cardColumns = new String[] { "Card #", "Front of Card", "Back of Card" };
            var cardRows = new List<string[]>();
            foreach (var card in flashcards)
            {
                var row = new String[] { index.ToString(), card.FrontOfCard, card.BackOfCard };
                cardRows.Add(row);
                index++;
            }

            DisplayUtils.DisplayListAsTable(cardColumns, cardRows);
        }

        public static void DisplayOneSideOfFlashcard(Flashcard card, bool displayFrontSide)
        {
            Table table = new();

            if (displayFrontSide)
            {
                table.AddColumn(card.FrontOfCard);
            }
            else
            {
                table.AddColumn(card.BackOfCard);
            }

            AnsiConsole.Write(table);
        }

        public static bool PromptForFlashcard(Flashcard flashcard, bool displayFrontSide)
        {
            DisplayOneSideOfFlashcard(flashcard, displayFrontSide);
            var input = DisplayUtils.PromptUserForStringInput("Input your answer to this card: ");
            string cardAnswer;

            cardAnswer = displayFrontSide ? flashcard.BackOfCard : flashcard.FrontOfCard;
            bool isCorrect = string.Equals(input.ToLower(), cardAnswer.ToLower());

            if (isCorrect)
            {
                AnsiConsole.WriteLine("Correct!");
                return true;
            }

            AnsiConsole.WriteLine("Incorrect. The correct answer was: \"" + cardAnswer + "\"");
            return false;
        }

        #endregion

        #region Study Session Display Methods

        public static void DisplayFinalScore(int score, int totalPossiblePoints)
        {
            AnsiConsole.WriteLine("Final Score: " + score.ToString() + " out of " + totalPossiblePoints.ToString() + " possible.");
            PressAnyKeyToContinue();
        }

        public static void DisplayStudySessions(List<StudySession> studySessions, Dictionary<int, string> stackNamesByID)
        {
            string[] columns = ["Study Session Date", "Card Stack", "Score"];
            List<string[]> rows = new();

            foreach (StudySession studySession in studySessions)
            {
                string[] temp = [$"{studySession.SessionDate.ToShortDateString()}", $"{stackNamesByID[studySession.StackId]}", $"{studySession.Score}"];
                rows.Add(temp);
            }

            DisplayUtils.DisplayListAsTable(columns, rows);
        }


        #endregion
    }
}
