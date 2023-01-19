using System;
using System.Collections.Generic;

namespace FlashStudy.Models
{
  public class Stack
  {
    public Stack(string stackName) {
      if(string.IsNullOrWhiteSpace(stackName))
        throw new Exception("Stack name cannot be empty");
      StackName = stackName;
    }

    public Stack(int stackId, string stackName) {
      if(string.IsNullOrWhiteSpace(stackName))
        throw new Exception("Stack name cannot be empty");

      try
      {
        StackId = Convert.ToInt32(stackId);
      }

      catch (FormatException)
      {
          throw new Exception("Please enter a valid number");
      }

      StackName = stackName;
    }

    public Stack(List<string> properties) {
      var stackId = properties[0];
      var stackName = properties[1];

      if(string.IsNullOrWhiteSpace(stackName))
        throw new Exception("Stack name cannot be empty");

        try {
          StackId = Convert.ToInt32(stackId);
        }

        catch (FormatException) {
            throw new Exception("Please enter a valid number");
        }

      StackName = stackName;
    }

    public int StackId { get; set; }
    public string StackName { get; set; }
  }

  class ReplaceStack
  {
    public string OldStackName { get; set; }
    public string NewStackName { get; set; }

    public ReplaceStack(string[] names)
    {
      try {
        OldStackName = names[0];
        NewStackName = names[1];
      }
      catch (IndexOutOfRangeException) {
        Console.WriteLine("New stack name cannot be null");
      }
    }
  }
}
