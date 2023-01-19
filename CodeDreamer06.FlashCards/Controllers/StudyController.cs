using System;
using System.Collections.Generic;
using FlashStudy.Utilities;
using FlashStudy.Models;
using FlashStudy.DTOs;

namespace FlashStudy.Controllers
{
  class StudyController
  {
    public static List<Stack> ShowStackList()
    {
      List<Stack> stacks = SqlAccess.ReadStacks();
      if(stacks.Count == 0) return new List<Stack>();

      Console.WriteLine("What would you like to study today?");
      var stackViews = new List<StackToViewDTO>();

      for(int i = 0; i < stacks.Count; i++)
        stackViews.Add(new StackToViewDTO(stacks[i]));

      for (int i = 0; i < stackViews.Count; i++)
        Console.WriteLine($"{i + 1}   {stackViews[i].StackName}");

      return stacks;
    }

    public static void Start() {
      var stacks = ShowStackList();

      if(stacks.Count == 0) {
        Console.WriteLine(Help.mainMenu);
        return;
      }

      string command = Console.ReadLine().ToLower().Trim();

      while(string.IsNullOrWhiteSpace(command)) {
        Console.WriteLine("Please enter a valid number");
        command = Console.ReadLine().ToLower().Trim();
      }

      if(command == "back" || command == "0") {
        Console.WriteLine(Help.mainMenu);
        return;
      }

      int stackId = Convert.ToInt32(command);
      int absoluteStackId = 0;

      try {
        absoluteStackId = stacks[stackId - 1].StackId;
      }

      catch {
        Console.WriteLine("Please choose a valid option from the list.");
        StudyController.Start();
      }

      var flashCards = SqlAccess.ReadFlashCards(absoluteStackId);

      int correctCount = 0;
      for (int i = 0; i < flashCards.Count; i++)
      {
          Console.WriteLine(flashCards[i].Title);
          string userAnswer = Console.ReadLine().ToLower().Trim();
          string correctAnswer = flashCards[i].Answer.ToLower();

          if(userAnswer == correctAnswer) correctCount += 1;
      }

      decimal score = Decimal.Divide(correctCount, flashCards.Count) * 100;
      Console.WriteLine("Today's Score: " + score + "%");
      SqlAccess.AddSession(new Session((int) score, absoluteStackId));

      Console.WriteLine(Help.mainMenu);
    }

    public static void ViewSessions()
    {
      var sessions = SqlAccess.ReadSessions();
      var sessionViews = new List<SessionToViewDTO>();

      for(int i = 0; i < sessions.Count; i++)
        sessionViews.Add(new SessionToViewDTO(sessions[i]));

      for(int i = 0; i < sessionViews.Count; i++)
        Console.WriteLine($"{i + 1}   {sessionViews[i].Score.ToString()}   {sessions[i].CreatedOn}");
      
      Console.WriteLine(Help.mainMenu);
    }
  }
}
