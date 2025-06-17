using System.ComponentModel.DataAnnotations;
using Flashcards.DTOs;
using Microsoft.IdentityModel.Tokens;
using Spectre.Console;
using static Flashcards.Enums;
namespace Flashcards
{
    internal abstract class Menu
    {
        protected bool exitApp = false;
        protected StackController stackController = new();
        protected FlashcardController flashcardController = new();
        protected StudySessionController studySessionController = new();

        internal abstract void PrintMenu();
    }

    internal class MainMenu : Menu
    {
        internal override void PrintMenu()
        {
            while (!exitApp)
            {
                Console.Clear();

                MainMenuOptions userSelection = AnsiConsole.Prompt(new SelectionPrompt<MainMenuOptions>().Title("What do you want to do?").
                    AddChoices(Enum.GetValues<MainMenuOptions>())
                     .UseConverter(option => option.GetAttribute<DisplayAttribute>()?.Name ?? option.ToString())); //? after GetAttribute is to prevent it from throwing exception if it returns NULL. ?? is null coerscing operator that reverts to the right hand side if left is null.
                switch (userSelection)
                {
                    case MainMenuOptions.Exit:
                        exitApp = true;
                        break;
                    case MainMenuOptions.Flashcards:
                        List<StackDTO> stackResults = stackController.GiveStacks();
                        if (stackResults.IsNullOrEmpty())
                        {
                            AnsiConsole.Markup("No stacks found!");
                            Console.ReadLine();
                            break;
                        }
                        string stackName = UI.StackSelector(stackResults);
                        FlashcardManagementMenu flashcardMenu = new(stackName);
                        flashcardMenu.PrintMenu();
                        break;
                    case MainMenuOptions.Manage_Stacks:
                        StackManagementMenu stackMenu = new();
                        stackMenu.PrintMenu();
                        break;
                    case MainMenuOptions.Study:
                        stackResults = stackController.GiveStacks();
                        if (stackResults.IsNullOrEmpty())
                        {
                            AnsiConsole.Markup("No stacks found!");
                            Console.ReadLine();
                            break;
                        }
                        stackName = UI.StackSelector(stackResults);
                        int dtoID = stackResults.FindIndex(dto => stackName == dto.Name) + 1;
                        StudyHandler(stackName, dtoID);
                        break;
                    case MainMenuOptions.View_Study_Session_Data:
                        List<StudySessionDTO> studySessions = studySessionController.GetStudySessions();
                        UI.ShowStudySessions(studySessions);
                        Console.ReadLine();
                        break;


                }
            }
        }

        private void StudyHandler(string stackName, int dtoID)
        {
            Random random = new Random();
            List<FlashcardDTO> flashcardDTOs = flashcardController.GiveStackFlashcards(stackName);
            if (flashcardDTOs.Count == 0)
            {
                AnsiConsole.Markup("No flashcards to study!");
                Console.ReadLine();
                return;
            }
            int score = 0;
            int numberOfFlashcards = flashcardDTOs.Count;
            while (flashcardDTOs.Count != 0)
            {
                Console.Clear();

                var count = flashcardDTOs.Count;
                int randIndex = random.Next(0, count);
                var temp = flashcardDTOs[randIndex];
                flashcardDTOs[randIndex] = flashcardDTOs[count - 1];
                flashcardDTOs[count - 1] = temp;
                var currentFlashcard = flashcardDTOs[count - 1];
                UI.ShowStudyFlashcard(flashcardDTOs[count - 1]);
                flashcardDTOs.RemoveAt(count - 1);

                string userAnswer = UI.GetUserAnswer();

                if (Validator.AnswerValidator(userAnswer, currentFlashcard.Back))
                {
                    AnsiConsole.Markup("[green] Correct Answer![/]");
                    score++;
                }
                else AnsiConsole.Markup($"[red] Wrong Answer![/] Correct answer is: {currentFlashcard.Back}");
                Console.ReadLine();

            }

            double finalPercentage = ((double)score / numberOfFlashcards * 100);
            (bool, string?) res = studySessionController.AddSession(dtoID, stackName, finalPercentage);

            if (res.Item1)
            {
                AnsiConsole.Markup("[green]Session added successfully![/]");
            }
            else AnsiConsole.Markup($"[red]Error in adding session: {res.Item2}[/]");
            Console.ReadLine();

        }



    }
    internal class FlashcardManagementMenu : Menu
    {
        string stackName;
        int id;

        internal FlashcardManagementMenu(string name)
        {
            stackName = name;
        }
        internal override void PrintMenu()
        {

            while (!exitApp)
            {
                Console.Clear();
                FlashcardManagementOptions userSelection = AnsiConsole.Prompt(new SelectionPrompt<FlashcardManagementOptions>()
                    .Title($"Current working stack: {stackName}")
                    .AddChoices(Enum.GetValues<FlashcardManagementOptions>())
                    .UseConverter(option => option.GetAttribute<DisplayAttribute>()?.Name ?? option.ToString()));


                switch (userSelection)
                {
                    case FlashcardManagementOptions.Main_Menu:
                        exitApp = true;
                        break;

                    case FlashcardManagementOptions.Current_Stack:
                        List<StackDTO> stackResults = stackController.GiveStacks();
                        if (stackResults.Count == 0)
                        {
                            AnsiConsole.Markup("No stacks found!");
                            Console.ReadLine();

                            break;
                        }
                        stackName = UI.StackSelector(stackResults);
                        break;

                    case FlashcardManagementOptions.View_Flashcards:

                        List<FlashcardDTO> flashcards = flashcardController.GiveStackFlashcards(stackName);
                        UI.ShowFlashcards(flashcards);
                        if (flashcards.Count == 0)
                        {
                            AnsiConsole.Markup("No Flashcards found!");
                            Console.ReadLine();

                            break;
                        }
                        Console.ReadLine();

                        break;
                    case FlashcardManagementOptions.Delete_Flashcard:
                        List<FlashcardDTO> flashcardResults = flashcardController.GiveStackFlashcards(stackName);
                        if (flashcardResults.Count == 0)
                        {
                            AnsiConsole.Markup("No flashcards found!");
                            Console.ReadLine();

                            break;
                        }
                        id = UI.FlashcardSelector(flashcardResults);
                        flashcardController.DeleteFlashcard(id, flashcardResults);
                        AnsiConsole.Markup("[green]Flashcard deleted successfully![/]");
                        Console.ReadLine();
                        break;

                    case FlashcardManagementOptions.View_X_Number_Of_Cards:
                        int numberOfFlashcards = AnsiConsole.Prompt(new TextPrompt<int>("Enter the number of flashcards you want to see: "));
                        List<FlashcardDTO> flashcardDTOs = flashcardController.GiveStackFlashcards(stackName);
                        if (flashcardDTOs.Count == 0)
                        {
                            AnsiConsole.Markup("No flashcards found!");
                            Console.ReadLine();

                            break;
                        }
                        UI.ShowFlashcards(flashcardDTOs, numberOfFlashcards);
                        Console.ReadLine();
                        break;

                    case FlashcardManagementOptions.Edit_Flashcard:
                        List<FlashcardDTO> flashCardResults = flashcardController.GiveStackFlashcards(stackName);
                        if (flashCardResults.Count == 0)
                        {
                            AnsiConsole.Markup("No flashcards found!");
                            Console.ReadLine();

                            break;
                        }
                        id = UI.FlashcardSelector(flashCardResults);
                        (string, EditType) updateValuePack = UI.AskForUpdateValue();
                        flashcardController.EditFlashcard(id, flashCardResults, updateValuePack.Item2, updateValuePack.Item1);
                        break;

                    case FlashcardManagementOptions.Create_Flashcard:
                        UI.ShowFlashcards(flashcardController.GiveStackFlashcards(stackName));
                        (string, string) flashcardInfo = UI.CreateFlashcard();
                        flashcardController.CreateFlashcard(flashcardInfo.Item1, flashcardInfo.Item2, stackName);
                        AnsiConsole.Markup("[green]Flashcard created successfully[/]");
                        break;

                }

            }





        }
    }


    internal class StackManagementMenu : Menu
    {
        internal override void PrintMenu()
        {
            while (!exitApp)
            {
                Console.Clear();
                StackManagementOptions userSelection = AnsiConsole.Prompt(new SelectionPrompt<StackManagementOptions>().Title("What do you want to do?").
                AddChoices(Enum.GetValues<StackManagementOptions>())
                 .UseConverter(option => option.GetAttribute<DisplayAttribute>()?.Name ?? option.ToString()));


                switch (userSelection)
                {
                    case StackManagementOptions.View:
                        List<StackDTO> stackDTOs = stackController.GiveStacks();
                        if (stackDTOs.Count == 0)
                        {
                            AnsiConsole.Markup("No stacks found!");
                            Console.ReadLine();

                            break;
                        }
                        UI.ShowStacks(stackDTOs);
                        Console.ReadLine();
                        break;
                    case StackManagementOptions.Exit:
                        exitApp = true;
                        break;
                    case StackManagementOptions.Add:
                        string stackName = UI.AddStack();
                        (bool, string?) res = stackController.CreateStack(stackName);
                        if (res.Item1)
                        {
                            AnsiConsole.Markup("[green]Stack created successfully![/]");
                        }
                        else AnsiConsole.Markup($"[red]Error in creating stack: {res.Item2}[/]");
                        Console.ReadLine();

                        break;

                    case StackManagementOptions.Edit:
                        stackDTOs = stackController.GiveStacks();
                        if (stackDTOs.Count == 0)
                        {
                            AnsiConsole.Markup("No stacks found!");
                            Console.ReadLine();

                            break;
                        }
                        stackName = UI.StackSelector(stackController.GiveStacks());
                        int index = stackDTOs.FindIndex(dto => stackName == dto.Name);
                        string newStackName = UI.AddStack();
                        res = stackController.EditStack(stackDTOs[index].ID, newStackName);
                        if (res.Item1)
                        {
                            AnsiConsole.Markup("[green]Stack edited successfully![/]");
                        }
                        else AnsiConsole.Markup($"[red]Error in editing stack: {res.Item2}[/]");
                        Console.ReadLine();
                        break;

                    case StackManagementOptions.Remove:
                        stackDTOs = stackController.GiveStacks();
                        if (stackDTOs.Count == 0)
                        {
                            AnsiConsole.Markup("Stacks not found!");
                            Console.ReadLine();
                            break;
                        }
                        stackName = UI.StackSelector(stackController.GiveStacks());
                        index = stackDTOs.FindIndex(dto => stackName == dto.Name);
                        res = stackController.RemoveStack(stackDTOs[index].ID);
                        if (res.Item1)
                        {
                            AnsiConsole.Markup("[green]Stack removed successfully![/]");
                        }
                        else AnsiConsole.Markup($"[red]Error in removing stack: {res.Item2}[/]");
                        Console.ReadLine();
                        break;


                }
            }




        }
    }

}


