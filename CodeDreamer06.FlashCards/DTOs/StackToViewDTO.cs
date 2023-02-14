using System;
using FlashStudy.Models;

namespace FlashStudy.DTOs
{
  public class StackToViewDTO
  {
    public string StackName { get; set; }

    public StackToViewDTO(Stack stack)
    {
      StackName = stack.StackName;
    }
  }
}
