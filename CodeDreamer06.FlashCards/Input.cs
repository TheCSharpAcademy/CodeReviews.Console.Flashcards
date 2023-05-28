using System;
using System.Collections.Generic;
using FlashStudy.Utilities;
using FlashStudy.Controllers;

namespace FlashStudy
{
  class Input
  {
    public static string command = "";

    public static void ShowMenu()
    {
      Console.WriteLine(Help.mainMenu);

      var actions = new Dictionary<int, Action>
      {
          { 1, StudyController.Start },
          { 2, StackController.Start },
          { 3, CardController.Start },
          { 4, StudyController.ViewSessions }
      };

      while(true)
      {
        command = Console.ReadLine().ToLower().Trim();

        if(command == "exit" || command == "0") break;
        else if(command == "help") Console.WriteLine(Help.mainMenu);
        else if(string.IsNullOrWhiteSpace(command)) continue;

        else
        {
          try {
            actions[Convert.ToInt32(command)]();
          }

          catch (KeyNotFoundException) {
            Console.WriteLine("Not a command. Use 'help' if required. ");
          }
        }
      }
    }
  }
}
