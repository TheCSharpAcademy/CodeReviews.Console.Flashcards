using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FlashCard.SheheryarRaza.DataBase;
using FlashCard.SheheryarRaza.Entities;
using System;
using System.Linq;
using System.Collections.Generic;

namespace FlashCard.SheheryarRaza.Managers
{
    public class StackManager
    {
        private readonly IServiceProvider _serviceProvider;

        public StackManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void ManageStacks()
        {
            bool managing = true;
            while (managing)
            {
                ConsoleHelper.ClearConsole();
                ConsoleHelper.DisplayMessage("--- Manage Stacks ---", ConsoleColor.Yellow);
                Console.WriteLine("1. Add New Stack");
                Console.WriteLine("2. View All Stacks");
                Console.WriteLine("3. Update Stack");
                Console.WriteLine("4. Delete Stack");
                Console.WriteLine("5. Back to Main Menu");
                ConsoleHelper.DisplayMessage("---------------------", ConsoleColor.Yellow);

                string choice = ConsoleHelper.GetStringInput("Enter your choice: ");

                switch (choice)
                {
                    case "1":
                        AddStack();
                        break;
                    case "2":
                        ViewAllStacks();
                        break;
                    case "3":
                        UpdateStack();
                        break;
                    case "4":
                        DeleteStack();
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

        public void AddStack()
        {
            ConsoleHelper.ClearConsole();
            ConsoleHelper.DisplayMessage("--- Add New Stack ---", ConsoleColor.Green);
            string name = ConsoleHelper.GetStringInput("Enter Stack Name (must be unique): ");
            string description = ConsoleHelper.GetStringInput("Enter Stack Description: ");

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DBContext>();
                try
                {
                    if (context.Stacks.Any(s => s.Name.ToLower() == name.ToLower()))
                    {
                        ConsoleHelper.DisplayMessage($"Stack with name '{name}' already exists. Please choose a different name.", ConsoleColor.Red);
                        ConsoleHelper.PressAnyKeyToContinue();
                        return;
                    }

                    var newStack = new Stacks
                    {
                        Name = name,
                        Description = description,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    context.Stacks.Add(newStack);
                    context.SaveChanges();
                    ConsoleHelper.DisplayMessage($"Stack '{name}' added successfully!", ConsoleColor.Green);
                }
                catch (Exception ex)
                {
                    ConsoleHelper.DisplayMessage($"Error adding stack: {ex.Message}", ConsoleColor.Red);
                }
            }
            ConsoleHelper.PressAnyKeyToContinue();
        }

        public void ViewAllStacks()
        {
            ConsoleHelper.ClearConsole();
            ConsoleHelper.DisplayMessage("--- All Stacks ---", ConsoleColor.Green);
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DBContext>();
                try
                {
                    var stacks = context.Stacks.OrderBy(s => s.Name).ToList();
                    if (!stacks.Any())
                    {
                        ConsoleHelper.DisplayMessage("No stacks found.", ConsoleColor.Yellow);
                        ConsoleHelper.PressAnyKeyToContinue();
                        return;
                    }

                    Console.WriteLine("ID\tName\t\tDescription");
                    Console.WriteLine("--\t----\t\t-----------");
                    foreach (var stack in stacks)
                    {
                        Console.WriteLine($"{stack.Id}\t{stack.Name}\t\t{stack.Description}");
                    }
                }
                catch (Exception ex)
                {
                    ConsoleHelper.DisplayMessage($"Error viewing stacks: {ex.Message}", ConsoleColor.Red);
                }
            }
            ConsoleHelper.PressAnyKeyToContinue();
        }

        public void UpdateStack()
        {
            ConsoleHelper.ClearConsole();
            ConsoleHelper.DisplayMessage("--- Update Stack ---", ConsoleColor.Green);
            ViewAllStacks();
            string stackName = ConsoleHelper.GetStringInput("Enter the Name of the stack to update: ");

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DBContext>();
                try
                {
                    var stackToUpdate = context.Stacks.FirstOrDefault(s => s.Name.ToLower() == stackName.ToLower());
                    if (stackToUpdate == null)
                    {
                        ConsoleHelper.DisplayMessage($"Stack with name '{stackName}' not found.", ConsoleColor.Red);
                        ConsoleHelper.PressAnyKeyToContinue();
                        return;
                    }

                    ConsoleHelper.DisplayMessage($"Current Name: {stackToUpdate.Name}", ConsoleColor.Yellow);
                    string newName = ConsoleHelper.GetStringInput("Enter New Name (leave empty to keep current): ");
                    if (!string.IsNullOrWhiteSpace(newName))
                    {
                        if (context.Stacks.Any(s => s.Name.ToLower() == newName.ToLower() && s.Id != stackToUpdate.Id))
                        {
                            ConsoleHelper.DisplayMessage($"New name '{newName}' already taken by another stack.", ConsoleColor.Red);
                            ConsoleHelper.PressAnyKeyToContinue();
                            return;
                        }
                        stackToUpdate.Name = newName;
                    }

                    ConsoleHelper.DisplayMessage($"Current Description: {stackToUpdate.Description}", ConsoleColor.Yellow);
                    string newDescription = ConsoleHelper.GetStringInput("Enter New Description (leave empty to keep current): ");
                    if (!string.IsNullOrWhiteSpace(newDescription))
                    {
                        stackToUpdate.Description = newDescription;
                    }

                    stackToUpdate.UpdatedAt = DateTime.UtcNow;
                    context.SaveChanges();
                    ConsoleHelper.DisplayMessage($"Stack '{stackToUpdate.Name}' updated successfully!", ConsoleColor.Green);
                }
                catch (Exception ex)
                {
                    ConsoleHelper.DisplayMessage($"Error updating stack: {ex.Message}", ConsoleColor.Red);
                }
            }
            ConsoleHelper.PressAnyKeyToContinue();
        }

        public void DeleteStack()
        {
            ConsoleHelper.ClearConsole();
            ConsoleHelper.DisplayMessage("--- Delete Stack ---", ConsoleColor.Green);
            ViewAllStacks();
            string stackName = ConsoleHelper.GetStringInput("Enter the Name of the stack to delete: ");

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DBContext>();
                try
                {
                    var stackToDelete = context.Stacks.FirstOrDefault(s => s.Name.ToLower() == stackName.ToLower());
                    if (stackToDelete == null)
                    {
                        ConsoleHelper.DisplayMessage($"Stack with name '{stackName}' not found.", ConsoleColor.Red);
                        ConsoleHelper.PressAnyKeyToContinue();
                        return;
                    }

                    ConsoleHelper.DisplayMessage($"WARNING: Deleting stack '{stackToDelete.Name}' will also delete all associated flashcards and study sessions.", ConsoleColor.Red);
                    string confirmation = ConsoleHelper.GetStringInput("Type 'YES' to confirm deletion: ");

                    if (confirmation.ToUpper() == "YES")
                    {
                        context.Stacks.Remove(stackToDelete);
                        context.SaveChanges();
                        ConsoleHelper.DisplayMessage($"Stack '{stackToDelete.Name}' and its related flashcards/study sessions deleted successfully!", ConsoleColor.Green);
                    }
                    else
                    {
                        ConsoleHelper.DisplayMessage("Deletion cancelled.", ConsoleColor.Yellow);
                    }
                }
                catch (Exception ex)
                {
                    ConsoleHelper.DisplayMessage($"Error deleting stack: {ex.Message}", ConsoleColor.Red);
                }
            }
            ConsoleHelper.PressAnyKeyToContinue();
        }
    }
}
