using System;
using System.Collections.Generic;
using FlashStudy.Utilities;
using FlashStudy.Models;
using FlashStudy.DTOs;

namespace FlashStudy.Controllers
{
  class CardController {
    public static string command = "";

    public static void Start()
    {
      Console.WriteLine(Help.cardMessage);
      while(true)
      {
        command = Console.ReadLine().ToLower().Trim();

        if(command == "show")
        {
          var flashCards = SqlAccess.ReadFlashCards();
          var flashCardsViews = new List<FlashCardToViewDTO>();

          for(int i = 0; i < flashCards.Count; i++)
              flashCardsViews.Add(new FlashCardToViewDTO(flashCards[i]));

          for(int i = 0; i < flashCardsViews.Count; i++) {
            var view = flashCardsViews[i];
            Console.WriteLine($"{i + 1}   {view.Title}     {view.Answer}");
          }
        }

        else if(command == "add")
        {
          var cardProperties = new string[] { "Title: ", "Answer: ", "Stack Name: " };
          var cardData = new string[3];

          Console.WriteLine("\nYour Stacks:");
          var stacks = SqlAccess.ReadStacks();
          for (int i = 0; i < stacks.Count; i++)
            Console.WriteLine($"{stacks[i].StackName}");
          Console.WriteLine();

          for (int i = 0; i < cardData.Length; i++) {
            Console.Write(cardProperties[i]);
            var userInput = Console.ReadLine().Trim();
            cardData[i] = userInput;

            if(i == 2)
            {
              foreach (var stack in stacks)
                if(stack.StackName == cardData[i])
                  cardData[i] = stack.StackId.ToString();

              // If the stackId hasn't been changed, it indicates that that the stack name is invalid
              if(cardData[i] == userInput) {
                Console.WriteLine("Please enter a valid stack name");
                CardController.Start();
              }
            }

            cardData[i] = userInput;
          }

          SqlAccess.AddCard(new FlashCard(cardData));
        }

        else if(command.StartsWith("remove"))
          SqlAccess.RemoveCard(
            StringUtilities.splitInteger(
              command,
              "remove",
              Help.cardRemoveErrorMessage));

        else if(command == "back" || command == "0") break;
        else if(command == "help") Console.WriteLine(Help.cardMessage);
        else if(string.IsNullOrWhiteSpace(command)) continue;
        else Console.WriteLine("Not a command. Use 'help' if required. ");

      }
      Console.WriteLine(Help.mainMenu);
    }
  }
}
