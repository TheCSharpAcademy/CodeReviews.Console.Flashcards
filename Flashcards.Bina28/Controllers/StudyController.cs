﻿


using Flashcards.Bina28.DBmanager;
using Flashcards.Bina28.Models;
using Spectre.Console;


namespace Flashcards.Bina28.Controllers;

internal class StudyController
{
	public List<StudySessionDto> studySession;
	private readonly Helper _inputHelper = new Helper();
	private readonly FlashcardsController _flashCard_controller = new();
	private readonly StudySessionDB db = new();

	public StudyController()
	{
		studySession = db.GetAllRecords() ?? new List<StudySessionDto>();
	}

	internal void StudySession()
	{
		int rightAnswer = 0;
		int wrongAnswer = 0;

		var sessionDate = DateTime.Now;

		// Refresh flashcards once before starting the study session
		_flashCard_controller.RefreshFlashCards(UserInterface.stackName);
		var flashcards = _flashCard_controller.GetFlashCards();

		// Check if there are any flashcards available
		if (flashcards.Count == 0)
		{
			Console.WriteLine("No flashcards available for the selected stack.");
			return;
		}

		bool continueStudy = true;

		while (continueStudy)
		{
			_flashCard_controller.DisplayAllFlashcards(UserInterface.stackName);
			int number = _inputHelper.GetValidIntegerInput("\nInput an ID of a flashcard \nor input 0 to exit");

			if (number == 0)
			{
				DisplaySessionStats(rightAnswer, wrongAnswer);
				break;
			}
			else if (!_flashCard_controller.IsCardExist(number))
			{
				Console.WriteLine("Flashcard not found. Please try again.");
				continue;
			}
			else
			{
				Console.Clear();
				ShowQuestion(number);
				string answer = _inputHelper.GetNonEmptyInput("\nInput your answer to this flashcard \nor input 0 to exit").ToLower();

				if (answer == "0")
				{
					DisplaySessionStats(rightAnswer, wrongAnswer);
					break;
				}

				if (IsRightAnswer(number, answer))
				{
					Console.WriteLine("Your answer is correct!");
					rightAnswer++;
				}
				else
				{
					Console.WriteLine($"Your answer is incorrect. Your answer: {answer}\nThe correct answer is: {flashcards[number - 1].Answer}");
					wrongAnswer++;
				}

				Console.WriteLine("\nPress any key to continue...");
				Console.ReadKey();
				Console.Clear();

			}
		}
		int totalQuestions = rightAnswer + wrongAnswer;

		db.CreateSession(sessionDate, UserInterface.stackName, wrongAnswer, rightAnswer, totalQuestions);

	}


	internal void DisplaySessionStats(int rightAnswer, int wrongAnswer)
	{
		int totalCount = rightAnswer + wrongAnswer;
		Console.WriteLine($"Current session status: You got {rightAnswer} correct out of {totalCount} flashcards.");
		Console.WriteLine("\nPress any key to continue...");
		Console.ReadKey();
	}

	internal bool IsRightAnswer(int cardId, string userAnswer)
	{
		_flashCard_controller.RefreshFlashCards(UserInterface.stackName);
		var flashcards = _flashCard_controller.GetFlashCards();
		int index = cardId - 1;
		var correctAnswer = flashcards[index].Answer.ToLower();

		return correctAnswer == userAnswer.ToLower();
	}

	internal void ShowQuestion(int number)
	{
		_flashCard_controller.RefreshFlashCards(UserInterface.stackName);
		var flashcards = _flashCard_controller.GetFlashCards();
		int index = number - 1;

		var table = new Table
		{
			Border = TableBorder.Rounded
		};

		table.AddColumn("[#ff007f]Question[/]");

		// Access the flashcard at the correct index
		table.AddRow($"[yellow]{flashcards[index].Question}[/]");

		AnsiConsole.Write(table);
	}

	internal void ViewSession()
	{
		var table = new Table
		{
			Border = TableBorder.Rounded
		};

		table.AddColumn("[#ff007f]Date[/]");
		table.AddColumn("[#ff007f]Stack name[/]");
		table.AddColumn("[#ff007f]Number of wrong answers[/]");
		table.AddColumn("[#ff007f]Number of right answers[/]");
		table.AddColumn("[#ff007f]Total number[/]");


		for (int i = 0; i < studySession.Count; i++)
		{
			table.AddRow($"[yellow]{studySession[i].SessionDate}[/]", $"[yellow]{studySession[i].StackName}[/]", $"[yellow]{studySession[i].NumberOfWrongAnswers}[/]", $"[yellow]{studySession[i].NumberOfRightAnswers}[/]", $"[yellow]{studySession[i].TotalQuestions}[/]");
		}
		AnsiConsole.Write(table);
	}

}

