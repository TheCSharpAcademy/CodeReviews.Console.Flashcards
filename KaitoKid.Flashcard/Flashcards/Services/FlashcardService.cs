using Flashcards.Repository;
using Flashcards.Views;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Services
{
    public class FlashcardService
    {
        private DatabaseContext _context;

        public FlashcardService(DatabaseContext _context)
        {
            this._context = _context;
        }

        public void SelectOperation(int opt, string stackName, int stackId)
        {
            var repo = new FlashcardRepository(_context, stackId);
            int cardId;
            UserInput userInput = new UserInput();
            try
            {


                switch (opt)
                {
                    case 3:
                        repo.GetAllCards();
                        break;

                    case 4:
                        Console.Write("How many flashcards you want to view?: ");
                        int limit = userInput.GetInt();
                        repo.GetXCards(limit);
                        break;

                    case 5:
                        string response;
                        do
                        {
                            Console.Write("Enter Question: ");
                            string question = userInput.GetText();
                            Console.Write("Enter Answer: ");
                            string answer = userInput.GetText();

                            repo.Insert(question, answer);

                            Console.Write("\nPress y to add another flashcard: ");
                            response = userInput.GetText();
                            Console.WriteLine();

                        } while (response.ToLower() == "y");
                        break;

                    case 6:
                        var view = new FlashcardView(_context);
                        if (repo.GetAllCards().Count == 0)
                        {
                            break;
                        }
                        int choice = view.UpdateMenu();
                        Console.Clear();
                        repo.GetAllCards();
                        switch (choice)
                        {
                            case 1:
                                Console.Write("Enter Card Id to be Updated: ");
                                cardId = userInput.GetInt();
                                Console.Write("Enter new question: ");
                                string question = userInput.GetText();
                                repo.UpdateQuestion(cardId, question);
                                break;

                            case 2:
                                Console.Write("Enter Card Id to be Updated: ");
                                cardId = userInput.GetInt();
                                Console.Write("Enter new answer: ");
                                string answer = userInput.GetText();
                                repo.UpdateAnswer(cardId, answer);
                                break;

                            case 3:
                                Console.Write("Enter Card Id to be Updated: ");
                                cardId = userInput.GetInt();
                                Console.Write("Enter new question: ");
                                question = userInput.GetText();
                                Console.Write("Enter new answer: ");
                                answer = userInput.GetText();

                                repo.UpdateQuestionAnswer(cardId, question, answer);
                                break;
                            case 4:
                                return;
                        }
                        break;

                    case 7:
                        repo.GetAllCards();
                        Console.Write("\nEnter Card Id to be Deleted: ");
                        cardId = userInput.GetInt();
                        repo.Delete(cardId);
                        break;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"\n[red]{ex.InnerException.Message}[/]\n\n");
                _context.ChangeTracker.Clear();
            }
            AnsiConsole.Markup("[blue]Press enter to continue....[/]");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
