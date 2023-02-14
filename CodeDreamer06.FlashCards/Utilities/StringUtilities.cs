using System;

namespace FlashStudy.Utilities
{
  class StringUtilities
  {
    public static int splitInteger(string word, string keyword, string errorMessage)
    {
      try
      {
        if(string.IsNullOrWhiteSpace(word.Split(keyword)[1]))
          throw new FormatException();

        return Convert.ToInt32(word.Split(keyword)[1]);
      }
      catch(System.FormatException)
      {
        Console.WriteLine(errorMessage);
        return 0;
      }
    }

    public static string SplitString(string word, string keyword, string errorMessage)
    {
      try
      {
        if(string.IsNullOrWhiteSpace(word.Split(keyword)[1]))
          throw new FormatException();

        return word.Split(keyword)[1].Trim();
      }
      catch(System.FormatException)
      {
        Console.WriteLine(errorMessage);
        return null;
      }
    }

    public static string[] SplitMultipleStrings(string word, string keyword, string errorMessage)
    {
      try
      {
        var stringArguments = word.Split(keyword)[1].Trim().Split();
        if(stringArguments.Length <= 1)
          throw new FormatException();

        return stringArguments;
      }
      catch(System.FormatException)
      {
        Console.WriteLine(errorMessage);
        return null;
      }
    }
  }
}
