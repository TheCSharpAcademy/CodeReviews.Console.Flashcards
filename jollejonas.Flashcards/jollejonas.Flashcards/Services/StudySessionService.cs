using jollejonas.Flashcards.Data;
using Spectre.Console;
using jollejonas.Flashcards.DTOs;

namespace jollejonas.Flashcards.Services
{
    public class StudySessionService(DatabaseManager databaseManager)
    {
        private readonly DatabaseManager _databaseManager = databaseManager;

        public void StartStudySession()
        {
            var cardStackService = new CardStackService(_databaseManager);
            var cardStackId = cardStackService.DisplayAndSelectCardStacks().Id;
            var cardStack = _databaseManager.GetCardStackDTOs(cardStackId);


            int correctAnswers = 0;
            int wrongAnswers = 0;
            foreach (var card in cardStack.Cards)
            {
                AnsiConsole.Markup($"[bold yellow] {cardStack.CardStackName}[/] \n\n");
                var questionPanel = new Panel($"Question: {card.Question}");
                questionPanel.Header = new PanelHeader($"Question number: {card.PresentationId}/{cardStack.Cards.Count}");

                AnsiConsole.Write(questionPanel);

                var userAnswer = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Do you know the answer?")
                    .PageSize(10)
                    .AddChoices("Yes", "No")
                    .UseConverter(option => option));

                if (userAnswer == "Yes")
                {
                    Console.WriteLine("Nice!\n");
                    correctAnswers++;
                }
                else
                {
                    Console.WriteLine("You might need to study this topic!\n");
                    wrongAnswers++;
                }

                Console.WriteLine();
                var answerPanel = new Panel($"Answer: {card.Answer}");
                answerPanel.Header = new PanelHeader("Answer");
                AnsiConsole.Write(answerPanel);

                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                Console.Clear();
            }
            Console.WriteLine($"Your result {correctAnswers}/{cardStack.Cards.Count()}");
            _databaseManager.RegisterStudySession(cardStackId, correctAnswers, wrongAnswers);
        }

        public void DisplaySessionsPerMonth()
        {
            int selectedYear = 0;
            var studySessions = new List<StudySessionPivotDto>();
            string displayType = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("What data you want to see?")
                .PageSize(10)
                .AddChoices("Sessions per month", "Average correct answers per month")
                .UseConverter(option => option));

            var table = new Table();
            if (displayType == "Sessions per month")
            {
                selectedYear = SelectYear();
                studySessions = _databaseManager.GetStudySessionsPerMonth(selectedYear);
                table.Title($"Sessions per month in {selectedYear.ToString()}");
            }
            else
            {
                selectedYear = SelectYear();
                studySessions = _databaseManager.GetAverageCorrectAnswersPerStudySessionPerMonth(selectedYear);
                table.Title($"Average correct answers per month in {selectedYear.ToString()}");
            }
            table.AddColumn("Stack name").Width(400);
            table.AddColumn("January");
            table.AddColumn("February");
            table.AddColumn("March");
            table.AddColumn("April");
            table.AddColumn("May");
            table.AddColumn("June");
            table.AddColumn("July");
            table.AddColumn("August");
            table.AddColumn("September");
            table.AddColumn("October");
            table.AddColumn("November");
            table.AddColumn("December");
            foreach(var session in studySessions)
            {
                table.AddRow(
                    session.StackName,
                    session.January.ToString(),
                    session.February.ToString(),
                    session.March.ToString(),
                    session.April.ToString(),
                    session.May.ToString(),
                    session.June.ToString(),
                    session.July.ToString(),
                    session.August.ToString(),
                    session.September.ToString(),
                    session.October.ToString(),
                    session.November.ToString(),
                    session.December.ToString()
                );
            }
            AnsiConsole.Write(table);
            Console.ReadKey();
        }

        public int SelectYear()
        {
            int year = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                .Title("Select year")
                .PageSize(10)
                .AddChoices(2024)
                .UseConverter(option => option.ToString()));

            return year;
        }


    }
}
