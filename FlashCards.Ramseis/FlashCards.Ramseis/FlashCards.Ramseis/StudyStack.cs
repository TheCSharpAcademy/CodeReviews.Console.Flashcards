namespace FlashCards.Ramseis
{
    internal class StudyStack
    {
        public static void Play(int stackID, string stackName)
        {
            Menu menu = new Menu
            {
                Titles = new List<string> { "-- Study Flashcards --", "Study Room", "Studying: " + stackName },
                Options = new List<string> { }
            };

            List<Card> cards = IO.SqlGetCards(stackID);

            bool escape = true;
            int questionNum = 1;
            int score = 0;

            menu.Options = new List<string>
                {
                    "Question " + questionNum + " of " + cards.Count,
                    "",
                    cards[questionNum-1].Question,
                    "",
                    "Enter answer below or Q to return to previous menu.",
                    "Score will not be saved if the stack is not completed!"
                };
            menu.Draw();

            while (escape)
            {       
                string input = (Console.ReadLine() ?? string.Empty).Trim();
                if (input == "q" | input == "Q")
                {
                    escape = false;
                }
                else if (input.Equals(cards[questionNum-1].Answer, StringComparison.OrdinalIgnoreCase))
                {
                    score++;
                    questionNum++;
                    if (questionNum <= cards.Count)
                    {
                        menu.Options = new List<string>
                        {
                            "Question " + (questionNum) + " of " + cards.Count,
                            "",
                            cards[questionNum-1].Question,
                            "",
                            "Enter answer below or Q to return to previous menu.",
                            "Score will not be saved if the stack is not completed!"
                        };
                        menu.Draw("Correct!");
                    } else
                    {
                        escape = false;
                        IO.SqlAddScore(score, cards.Count, stackName, stackID);
                    }
                }
                else
                {
                    questionNum++;
                    if (questionNum <= cards.Count)
                    {
                        menu.Options = new List<string>
                        {
                            "Question " + (questionNum) + " of " + cards.Count,
                            "",
                            cards[questionNum-1].Question,
                            "",
                            "Enter answer below or Q to return to previous menu.",
                            "Score will not be saved if the stack is not completed!"
                        };
                        menu.Draw("Incorrect!\n The answer was: " + cards[questionNum - 2].Answer);
                    }
                    else
                    {
                        IO.SqlAddScore(score, cards.Count, stackName, stackID);
                        escape = false;
                    }
                }
            }

            menu.Options = new List<string>
            {
                $"Flash card stack {stackName} completed!",
                $"Score: {score} / {cards.Count}    : {Math.Round(100f * score/cards.Count)}%",
                DateTime.Now.ToString()
            };
            
            menu.Draw("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
