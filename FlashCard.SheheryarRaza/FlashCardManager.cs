using System;
using System.Collections.Generic;
using System.Linq;
using FlashCard.SheheryarRaza.DataBase;
using FlashCard.SheheryarRaza.DTOs;
using FlashCard.SheheryarRaza.Entities;
using FlashCard.SheheryarRaza.Managers; // To access StackManager
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FlashCard.SheheryarRaza.Managers
{
    public class FlashcardManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly StackManager _stackManager;

        public FlashcardManager(IServiceProvider serviceProvider, StackManager stackManager)
        {
            _serviceProvider = serviceProvider;
            _stackManager = stackManager;
        }

        public void ManageFlashcards()
        {
            bool managing = true;
            while (managing)
            {
                ConsoleHelper.ClearConsole();
                ConsoleHelper.DisplayMessage("--- Manage Flashcards ---", ConsoleColor.Yellow);
                Console.WriteLine("1. Add New Flashcard to a Stack");
                Console.WriteLine("2. View Flashcards in a Stack");
                Console.WriteLine("3. Update Flashcard");
                Console.WriteLine("4. Delete Flashcard");
                Console.WriteLine("5. Back to Main Menu");
                ConsoleHelper.DisplayMessage("-------------------------", ConsoleColor.Yellow);

                string choice = ConsoleHelper.GetStringInput("Enter your choice: ");

                switch (choice)
                {
                    case "1":
                        AddFlashcard();
                        break;
                    case "2":
                        ViewFlashcardsInStack();
                        break;
                    case "3":
                        UpdateFlashcard();
                        break;
                    case "4":
                        DeleteFlashcard();
                        break;
                    case "5":
                        managing = false;
                        break;
                    default:
                        ConsoleHelper.DisplayMessage("Invalid choice. Please try again.", ConsoleColor.Red);
                        ConsoleHelper.PressAnyKeyToContinue();
                        break;
                }
            }
        }

        public void AddFlashcard()
        {
            ConsoleHelper.ClearConsole();
            ConsoleHelper.DisplayMessage("--- Add New Flashcard ---", ConsoleColor.Green);
            _stackManager.ViewAllStacks();
            string stackName = ConsoleHelper.GetStringInput("Enter the Name of the stack to add the flashcard to: ");

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DBContext>();
                try
                {
                    var stack = context.Stacks.FirstOrDefault(s => s.Name.ToLower() == stackName.ToLower());
                    if (stack == null)
                    {
                        ConsoleHelper.DisplayMessage($"Stack with name '{stackName}' not found. Please create it first.", ConsoleColor.Red);
                        ConsoleHelper.PressAnyKeyToContinue();
                        return;
                    }

                    string question = ConsoleHelper.GetStringInput("Enter Flashcard Question: ");
                    string answer = ConsoleHelper.GetStringInput("Enter Flashcard Answer: ");

                    var newFlashcard = new FlashCards
                    {
                        Question = question,
                        Answer = answer,
                        StackId = stack.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    context.FlashCards.Add(newFlashcard);
                    context.SaveChanges();
                    ConsoleHelper.DisplayMessage($"Flashcard added successfully to stack '{stack.Name}'!", ConsoleColor.Green);
                }
                catch (Exception ex)
                {
                    ConsoleHelper.DisplayMessage($"Error adding flashcard: {ex.Message}", ConsoleColor.Red);
                }
            }
            ConsoleHelper.PressAnyKeyToContinue();
        }

        public void ViewFlashcardsInStack()
        {
            ConsoleHelper.ClearConsole();
            ConsoleHelper.DisplayMessage("--- View Flashcards in a Stack ---", ConsoleColor.Green);
            _stackManager.ViewAllStacks();
            string stackName = ConsoleHelper.GetStringInput("Enter the Name of the stack to view flashcards: ");

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DBContext>();
                try
                {
                    var stack = context.Stacks.Include(s => s.FlashCards)
                                        .FirstOrDefault(s => s.Name.ToLower() == stackName.ToLower());

                    if (stack == null)
                    {
                        ConsoleHelper.DisplayMessage($"Stack with name '{stackName}' not found.", ConsoleColor.Red);
                        ConsoleHelper.PressAnyKeyToContinue();
                        return;
                    }

                    ConsoleHelper.DisplayMessage($"Flashcards for Stack: '{stack.Name}'", ConsoleColor.Yellow);

                    var flashcards = stack.FlashCards.OrderBy(fc => fc.Id).ToList();

                    if (!flashcards.Any())
                    {
                        ConsoleHelper.DisplayMessage("No flashcards found in this stack.", ConsoleColor.Yellow);
                        ConsoleHelper.PressAnyKeyToContinue();
                        return;
                    }

                    var flashcardDtos = flashcards
                        .Select((fc, index) => new FlashcardDTO
                        {
                            DisplayId = index + 1,
                            Question = fc.Question,
                            Answer = fc.Answer
                        })
                        .ToList();

                    Console.WriteLine("ID\tQuestion\tAnswer");
                    Console.WriteLine("--\t--------\t------");
                    foreach (var dto in flashcardDtos)
                    {
                        Console.WriteLine($"{dto.DisplayId}\t{dto.Question}\t\t{dto.Answer}");
                    }
                }
                catch (Exception ex)
                {
                    ConsoleHelper.DisplayMessage($"Error viewing flashcards: {ex.Message}", ConsoleColor.Red);
                }
            }
            ConsoleHelper.PressAnyKeyToContinue();
        }

        public void UpdateFlashcard()
        {
            ConsoleHelper.ClearConsole();
            ConsoleHelper.DisplayMessage("--- Update Flashcard ---", ConsoleColor.Green);
            _stackManager.ViewAllStacks();
            string stackName = ConsoleHelper.GetStringInput("Enter the Name of the stack containing the flashcard: ");

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DBContext>();
                try
                {
                    var stack = context.Stacks.Include(s => s.FlashCards)
                                        .FirstOrDefault(s => s.Name.ToLower() == stackName.ToLower());

                    if (stack == null)
                    {
                        ConsoleHelper.DisplayMessage($"Stack with name '{stackName}' not found.", ConsoleColor.Red);
                        ConsoleHelper.PressAnyKeyToContinue();
                        return;
                    }

                    var flashcardsForDisplay = stack.FlashCards.OrderBy(fc => fc.Id).ToList();
                    if (!flashcardsForDisplay.Any())
                    {
                        ConsoleHelper.DisplayMessage("No flashcards in this stack to update.", ConsoleColor.Yellow);
                        ConsoleHelper.PressAnyKeyToContinue();
                        return;
                    }

                    ConsoleHelper.DisplayMessage($"Flashcards in '{stack.Name}':", ConsoleColor.Yellow);
                    Console.WriteLine("Display ID\tQuestion");
                    Console.WriteLine("----------\t--------");
                    for (int i = 0; i < flashcardsForDisplay.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}\t\t{flashcardsForDisplay[i].Question}");
                    }

                    int displayIdToUpdate = ConsoleHelper.GetIntInput("Enter the Display ID of the flashcard to update: ");

                    if (displayIdToUpdate < 1 || displayIdToUpdate > flashcardsForDisplay.Count)
                    {
                        ConsoleHelper.DisplayMessage("Invalid Display ID.", ConsoleColor.Red);
                        ConsoleHelper.PressAnyKeyToContinue();
                        return;
                    }

                    var flashcardToUpdate = flashcardsForDisplay[displayIdToUpdate - 1];

                    ConsoleHelper.DisplayMessage($"Current Question: {flashcardToUpdate.Question}", ConsoleColor.Yellow);
                    string newQuestion = ConsoleHelper.GetStringInput("Enter New Question (leave empty to keep current): ");
                    if (!string.IsNullOrWhiteSpace(newQuestion))
                    {
                        flashcardToUpdate.Question = newQuestion;
                    }

                    ConsoleHelper.DisplayMessage($"Current Answer: {flashcardToUpdate.Answer}", ConsoleColor.Yellow);
                    string newAnswer = ConsoleHelper.GetStringInput("Enter New Answer (leave empty to keep current): ");
                    if (!string.IsNullOrWhiteSpace(newAnswer))
                    {
                        flashcardToUpdate.Answer = newAnswer;
                    }

                    flashcardToUpdate.UpdatedAt = DateTime.UtcNow;
                    context.SaveChanges();
                    ConsoleHelper.DisplayMessage($"Flashcard updated successfully!", ConsoleColor.Green);
                }
                catch (Exception ex)
                {
                    ConsoleHelper.DisplayMessage($"Error updating flashcard: {ex.Message}", ConsoleColor.Red);
                }
            }
            ConsoleHelper.PressAnyKeyToContinue();
        }

        public void DeleteFlashcard()
        {
            ConsoleHelper.ClearConsole();
            ConsoleHelper.DisplayMessage("--- Delete Flashcard ---", ConsoleColor.Green);
            _stackManager.ViewAllStacks();
            string stackName = ConsoleHelper.GetStringInput("Enter the Name of the stack containing the flashcard: ");

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DBContext>();
                try
                {
                    var stack = context.Stacks.Include(s => s.FlashCards)
                                        .FirstOrDefault(s => s.Name.ToLower() == stackName.ToLower());

                    if (stack == null)
                    {
                        ConsoleHelper.DisplayMessage($"Stack with name '{stackName}' not found.", ConsoleColor.Red);
                        ConsoleHelper.PressAnyKeyToContinue();
                        return;
                    }

                    var flashcardsForDisplay = stack.FlashCards.OrderBy(fc => fc.Id).ToList();
                    if (!flashcardsForDisplay.Any())
                    {
                        ConsoleHelper.DisplayMessage("No flashcards in this stack to delete.", ConsoleColor.Yellow);
                        ConsoleHelper.PressAnyKeyToContinue();
                        return;
                    }

                    ConsoleHelper.DisplayMessage($"Flashcards in '{stack.Name}':", ConsoleColor.Yellow);
                    Console.WriteLine("Display ID\tQuestion");
                    Console.WriteLine("----------\t--------");
                    for (int i = 0; i < flashcardsForDisplay.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}\t\t{flashcardsForDisplay[i].Question}");
                    }

                    int displayIdToDelete = ConsoleHelper.GetIntInput("Enter the Display ID of the flashcard to delete: ");

                    if (displayIdToDelete < 1 || displayIdToDelete > flashcardsForDisplay.Count)
                    {
                        ConsoleHelper.DisplayMessage("Invalid Display ID.", ConsoleColor.Red);
                        ConsoleHelper.PressAnyKeyToContinue();
                        return;
                    }

                    var flashcardToDelete = flashcardsForDisplay[displayIdToDelete - 1];

                    context.FlashCards.Remove(flashcardToDelete);
                    context.SaveChanges();
                    ConsoleHelper.DisplayMessage($"Flashcard '{flashcardToDelete.Question}' deleted successfully!", ConsoleColor.Green);
                }
                catch (Exception ex)
                {
                    ConsoleHelper.DisplayMessage($"Error deleting flashcard: {ex.Message}", ConsoleColor.Red);
                }
            }
            ConsoleHelper.PressAnyKeyToContinue();
        }
    }
}
