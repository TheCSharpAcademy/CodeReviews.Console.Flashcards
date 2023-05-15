using System;

namespace FlashStudy.Models
{
    public class FlashCard
    {

      public int CardId { get; set; }
      public string Title { get; set; }
      public string Answer { get; set; }
      public int StackId { get; set; }

      public FlashCard(string[] properties)
      {
        try
        {
          Title = properties[0];
          Answer = properties[1];
          StackId = Convert.ToInt32(properties[2]);
          if(properties.Length > 3) CardId = Convert.ToInt32(properties[3]);
        }

        catch {
          Console.WriteLine("Unable to create a FlashCard");
        }
      }

      public FlashCard(string title, string answer, string stackId)
      {
        Title = title;
        Answer = answer;
        StackId = Convert.ToInt32(stackId);
      }

      public FlashCard(string cardId, string title, string answer, string stackId)
      {
        CardId = Convert.ToInt32(cardId);
        Title = title;
        Answer = answer;
        StackId = Convert.ToInt32(stackId);
      }
    }
}
