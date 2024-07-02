using Flashcards.ukpagrace.Database;
using Flashcards.ukpagrace.DTO;
using Flashcards.ukpagrace.Entity;
using Flashcards.ukpagrace.Utility;
using Spectre.Console;

namespace Flashcards.ukpagrace.Controller
{
    class FlashcardController
    {
        FlashCardDatabase flashcardDatabase = new ();
        StackDatabase stackDatabase = new ();
        UserInput userInput = new();
        bool hasFlashcards = false;
        List<FlashcardDTO> records = new List<FlashcardDTO>();
        public void ShowFlashCards()
        {
            try
            {
                var stackName = userInput.GetStackOption();

                //List<FlashcardDTO> records = new List<FlashcardDTO>();
                int stackId = stackDatabase.GetStackId(stackName);
                records = flashcardDatabase.GetFlashcards(stackId);

                var table = new Table();
                table.Centered();
                table.Title(stackName).Centered();
                table.AddColumn(new TableColumn("[Red]Id[/]").Centered());
                table.AddColumn(new TableColumn("[Red]Question[/]").Centered());
                table.AddColumn(new TableColumn("[Red]Answer[/]").Centered());

                int count = 1;
                
                if(records.Count > 0)
                {
                    foreach (FlashcardDTO record in records)
                    {
                        record.Id = count++;
                        table.AddRow(
                            new Markup($"[blue]{record.Id}[/]"),
                            new Markup($"[blue]{record.Question}[/]"),
                            new Markup($"[blue]{record.Answer}[/]")
                        );
                    }
                    AnsiConsole.Write(table);
                    hasFlashcards = true;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red] No FlashCards found[/]"); 
                }

            }
            catch(Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]{ex}[/]");
            }
        }

        public void CreateTable()
        {
            flashcardDatabase.CreateFlashcard();
        }
        public void CreateFlashcard()
        {
            var stackName = userInput.GetStackOption();
            var question = userInput.GetQuestionInput();
            var answer = userInput.GetAnswerInput();
            int stackId = stackDatabase.GetStackId(stackName);
            flashcardDatabase.Insert(stackId, question, answer);
        }

        public void UpdateFlashcard()
        {
            ShowFlashCards();
            if (hasFlashcards)
            {
                int id = userInput.GetFlashCardInput();

                var flashcard = records.Find(x => x.Id == id);
                int flashcardId;
                if (flashcard == null){
                    AnsiConsole.MarkupLine("[red]Id does not exists[/]");
                    return;
                }
                flashcardId = flashcard.FlashcardId;

                if (!flashcardDatabase.IdExists(flashcardId))
                {
                    AnsiConsole.MarkupLine("[red]Id does not exists[/]");
                    return;
                }
                var updateOption = userInput.GetUpdateInput();
                FlashcardEntity record = flashcardDatabase.GetOne(flashcardId);
                if (updateOption.ToLower() == "question")
                {
                    string question = userInput.GetQuestionInput();
                    flashcardDatabase.Update(flashcardId, record.StackId, question, record.Answer);
                }
                else if (updateOption.ToLower() == "answer")
                {
                    string answer = userInput.GetAnswerInput();
                    flashcardDatabase.Update(flashcardId, record.StackId, record.Question, answer);

                }
                else
                {
                    string stackName = userInput.GetStackOption();
                    int stackId = stackDatabase.GetStackId(stackName);
                    flashcardDatabase.Update(flashcardId, stackId, record.Question, record.Answer);
                }
            }

        }

        public void DeleteFlashcard()
        {
            ShowFlashCards();
            if (hasFlashcards)
            {
                int id = userInput.GetFlashCardInput();
                int flashcardId = records.Find(x => x.Id == id).FlashcardId;
                if (!flashcardDatabase.IdExists(flashcardId))
                {
                    AnsiConsole.MarkupLine("[red]Id does not exists[/]");
                    return;
                }
                flashcardDatabase.Delete(flashcardId);
            }
        }
    }
}