using System;
using System.Collections.Generic;
using FlashStudy.Utilities;
using FlashStudy.Models;
using FlashStudy.DTOs;

namespace FlashStudy.Controllers
{
  class StackController
  {
    public static string command = "";

    public static void Start()
    {
      Console.WriteLine(Help.stackMessage);
      while(true)
      {
        command = Console.ReadLine().ToLower().Trim();

        if(command == "show") {
          var stacks = SqlAccess.ReadStacks();
          var stackViews = new List<StackToViewDTO>();

          for(int i = 0; i < stacks.Count; i++)
            stackViews.Add(new StackToViewDTO(stacks[i]));

          for (int i = 0; i < stackViews.Count; i++)
            Console.WriteLine($"{i + 1}   {stackViews[i].StackName}");
        }

        else if(command.StartsWith("add")) {
          string stackName = StringUtilities.SplitString(command, "add", Help.stackAddErrorMessage);
          if(string.IsNullOrWhiteSpace(stackName)) continue;

          SqlAccess.AddStack(new Stack(stackName));
        }

        else if(command.StartsWith("remove")) {
          string stackName = StringUtilities.SplitString(command, "remove", Help.stackRemoveErrorMessage);
          if(stackName == null) continue;

          SqlAccess.RemoveStack(new Stack(stackName));
        }

        else if(command.StartsWith("edit")) {
          var editArguments = StringUtilities.SplitMultipleStrings(command, "edit", Help.stackEditErrorMessage);
          if(editArguments == null) continue;

          SqlAccess.EditStack(new ReplaceStack(editArguments));
        }

        else if(command == "back" || command == "0") break;
        else if(command == "help") Console.WriteLine(Help.stackMessage);
        else if(string.IsNullOrWhiteSpace(command)) continue;
        else Console.WriteLine("Not a command. Use 'help' if required. ");
      }
      Console.WriteLine(Help.mainMenu);
    }
  }
}
